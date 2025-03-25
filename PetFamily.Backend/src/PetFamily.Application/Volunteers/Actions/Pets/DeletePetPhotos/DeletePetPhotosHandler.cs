using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Database;
using PetFamily.Application.FileProvider;
using PetFamily.Application.Providers;
using PetFamily.Application.Volunteers.Actions.Pets.AddPet;
using PetFamily.Domain.PetManagement.PetVO;
using PetFamily.Domain.PetManagement.VolunteerVO;
using PetFamily.Domain.Shared.ErrorContext;

namespace PetFamily.Application.Volunteers.Actions.Pets.DeletePetPhotos;

public class DeletePetPhotosHandler
{
    private const string BUCKET_NAME = "photos";
    
    private readonly IFileProvider _fileProvider;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly ILogger<AddPetHandler> _logger;
    
    public DeletePetPhotosHandler(
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
        DeletePetPhotosCommand command,
        CancellationToken cancellationToken = default)
    {
        var volunteerResult = await _volunteersRepository.GetById(
            VolunteerId.Create(command.VolunteerId),
            cancellationToken);
        
        var pet = volunteerResult.Value.Pets
            .FirstOrDefault(p => p.Id == command.PetId);
        
        if(pet == null)
            return Errors.General.NotFound(command.PetId);

        var photosIdToDelete = command.PhotosId.ToList();

        List<PetPhoto> photos = [];
        
        foreach (var photo in pet.PetPhotos)
        {
            var photoId = ExtractGuidFromPath(photo.PathToStorage.Path);

            if(photosIdToDelete.Contains(photoId))
            {
                photos.Add(photo);
            }
        }
        
        var photosPathWithBucket = new PhotosPathWithBucket(
            photos.Select(p => p.PathToStorage).ToList(), 
            BUCKET_NAME);
        
        var deleteResult = await _fileProvider.DeleteFiles(photosPathWithBucket, cancellationToken);
        if(deleteResult.IsFailure)
            return deleteResult.Error;

        pet.PetPhotos.RemoveAll(photos);
        
        await _unitOfWork.SaveChanges(cancellationToken);
        
        _logger.LogInformation("Deleted photos to pet with id {PetId} from a volunteer with id {VolunteerId}",
            command.PetId, command.VolunteerId);
        
        return pet.Id.Value;
    }
    
    private Guid ExtractGuidFromPath(string path)
    {
        string fileName = Path.GetFileName(path);
        var match = Regex.Match(fileName, @"^([a-fA-F0-9\-]+)");

        return match.Success && Guid.TryParse(match.Value, out var id) ? id : Guid.Empty;
    }
}