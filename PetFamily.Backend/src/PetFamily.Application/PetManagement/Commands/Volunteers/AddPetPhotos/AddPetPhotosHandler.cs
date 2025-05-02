using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Database;
using PetFamily.Application.Extensions;
using PetFamily.Application.Messaging;
using PetFamily.Application.Photos;
using PetFamily.Application.Providers;
using PetFamily.Domain.PetManagement.PetVO;
using PetFamily.Domain.PetManagement.VolunteerVO;
using PetFamily.Domain.Shared.ErrorContext;

namespace PetFamily.Application.PetManagement.Commands.Volunteers.AddPetPhotos;

public class AddPetPhotosHandler : ICommandHandler<Guid, AddPetPhotosCommand>
{
    private const string BUCKET_NAME = "photos";

    private readonly IFileProvider _fileProvider;
    private readonly IValidator<AddPetPhotosCommand> _addPetPhotosValidator;
    private readonly IMessageQueue<IEnumerable<PhotoInfo>> _messageQueue;
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly ILogger<AddPet.AddPet> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public AddPetPhotosHandler(
        IFileProvider fileProvider,
        IValidator<AddPetPhotosCommand> addPetPhotosValidator,
        IMessageQueue<IEnumerable<PhotoInfo>> messageQueue,
        IVolunteersRepository volunteersRepository,
        ILogger<AddPet.AddPet> logger,
        IUnitOfWork unitOfWork)
    {
        _fileProvider = fileProvider;
        _addPetPhotosValidator = addPetPhotosValidator;
        _messageQueue = messageQueue;
        _volunteersRepository = volunteersRepository;
        _logger = logger;
        _unitOfWork = unitOfWork;
    }


    public async Task<Result<Guid, ErrorList>> Handle(
        AddPetPhotosCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _addPetPhotosValidator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToErrorList();
        
        var transaction = await _unitOfWork.BeginTransaction(cancellationToken);
        try
        {
            var volunteerResult = await _volunteersRepository.GetById(
                VolunteerId.Create(command.VolunteerId),
                cancellationToken);
            if (volunteerResult.IsFailure)
                return volunteerResult.Error.ToErrorList();

            var petResult = volunteerResult.Value.GetPetById(command.PetId);
            
            List<PhotoData> photosData = [];

            foreach (var file in command.Photos)
            {
                var fullFileName = Path.GetFileName(file.PhotoName);

                var photoPath = PhotoPath.Create(Guid.NewGuid(), fullFileName);
                if (photoPath.IsFailure)
                    return photoPath.Error.ToErrorList();

                var fileData = new PhotoData(file.Content, new PhotoInfo(photoPath.Value, BUCKET_NAME));

                photosData.Add(fileData);
            }

            var petPhotos = photosData
                .Select(p => p.PhotoInfo.PhotoPath)
                .Select(p => PetPhoto.Create(p).Value)
                .ToList();

            petResult.Value.UpdatePetPhotos(petPhotos);

            await _unitOfWork.SaveChanges(cancellationToken);

            var uploadResult = await _fileProvider.UploadPhotos(photosData, cancellationToken);
            if (uploadResult.IsFailure)
            {
                await _messageQueue.WriteAsync(photosData
                    .Select(p => p.PhotoInfo), cancellationToken);
                return uploadResult.Error.ToErrorList();
            }

            transaction.Commit();

            _logger.LogInformation("Adding photos to pet with id {PetId} from a volunteer with id {VolunteerId}",
                command.PetId, command.VolunteerId);

            return petResult.Value.Id.Value;
        }

        catch (Exception exception)
        {
            _logger.LogError(exception,
                "Failed to add photos to pet with id {PetId} from volunteer with id {VolunteerId} in transaction",
                command.PetId, command.VolunteerId);
        }

        transaction.Rollback();

        return command.PetId;
    }
}