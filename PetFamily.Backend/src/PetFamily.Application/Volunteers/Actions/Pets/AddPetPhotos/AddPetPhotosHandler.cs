using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Database;
using PetFamily.Application.FileProvider;
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
    private readonly IUnitOfWork _unitOfWork;
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly ILogger<AddPetHandler> _logger;

    public AddPetPhotosHandler(
        IFileProvider fileProvider,
        IUnitOfWork unitOfWork,
        IVolunteersRepository volunteersRepository,
        ILogger<AddPetHandler> logger)
    {
        _fileProvider = fileProvider;
        _unitOfWork = unitOfWork;
        _volunteersRepository = volunteersRepository;
        _logger = logger;
    }
    
    
    public async Task<Result<Guid, Error>> Handle(
        AddPetPhotosCommand command,
        CancellationToken cancellationToken = default)
    {
        var transaction = await _unitOfWork.BeginTransaction(cancellationToken);

        try
        {
            var volunteerResult = await _volunteersRepository.GetById(
                VolunteerId.Create(command.VolunteerId),
                cancellationToken);

            var petResult = volunteerResult.Value.GetPetById(command.PetId);

            if (volunteerResult.IsFailure)
                return volunteerResult.Error;


            List<PhotoData> photosData = [];

            foreach (var file in command.CollectionFiles.Files)
            {
                var fullFileName = Path.GetFileName(file.FileName);

                var photoPath = PhotoPath.Create(Guid.NewGuid(), fullFileName);
                if (photoPath.IsFailure)
                    return photoPath.Error;

                var fileData = new PhotoData(file.Content, photoPath.Value, BUCKET_NAME);

                photosData.Add(fileData);
            }

            var petPhotos = photosData
                .Select(p => p.PhotoPath)
                .Select(p => PetPhoto.Create(p).Value)
                .ToList();

            petResult.Value.UpdatePetPhotos(petPhotos);

            await _unitOfWork.SaveChanges(cancellationToken);

            var uploadResult = await _fileProvider.UploadFiles(photosData, cancellationToken);

            if (uploadResult.IsFailure)
                return uploadResult.Error;

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
    
        return Error.Failure($"Failed to add photos to pet with id {command.PetId} for volunteer with id {command.VolunteerId}", "volunteer.pet.photos.failure");
    }
}