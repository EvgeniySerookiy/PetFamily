using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Database;
using PetFamily.Application.Extensions;
using PetFamily.Application.Messaging;
using PetFamily.Application.Photos;
using PetFamily.Application.Providers;
using PetFamily.Application.Volunteers.Actions.Pets.AddPet;
using PetFamily.Domain.PetManagement.PetVO;
using PetFamily.Domain.PetManagement.VolunteerVO;
using PetFamily.Domain.Shared.ErrorContext;

namespace PetFamily.Application.Volunteers.Actions.Pets.AddPetPhotos;

public class AddPetPhotosHandler
{
    private const string BUCKET_NAME = "photos";

    private readonly IFileProvider _fileProvider;
    private readonly IValidator<AddPetPhotosCommand> _addPetPhotosValidator;
    private readonly IMessageQueue<IEnumerable<PhotoInfo>> _messageQueue;
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly ILogger<AddPetHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public AddPetPhotosHandler(
        IFileProvider fileProvider,
        IValidator<AddPetPhotosCommand> addPetPhotosValidator,
        IMessageQueue<IEnumerable<PhotoInfo>> messageQueue,
        IVolunteersRepository volunteersRepository,
        ILogger<AddPetHandler> logger,
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
        Guid volunteerId,
        Guid petId,
        AddPetPhotosCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _addPetPhotosValidator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToErrorList();
        // Транзакция не нужна уже  
        var transaction = await _unitOfWork.BeginTransaction(cancellationToken);
        try
        {
            var volunteerResult = await _volunteersRepository.GetById(
                VolunteerId.Create(volunteerId),
                cancellationToken);

            var petResult = volunteerResult.Value.GetPetById(petId);

            if (volunteerResult.IsFailure)
                return volunteerResult.Error.ToErrorList();
            
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
                petId, volunteerId);

            return petResult.Value.Id.Value;
        }

        catch (Exception exception)
        {
            _logger.LogError(exception,
                "Failed to add photos to pet with id {PetId} from volunteer with id {VolunteerId} in transaction",
                petId, volunteerId);
        }

        transaction.Rollback();

        return petId;
    }
}