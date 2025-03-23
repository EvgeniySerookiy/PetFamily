using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;
using PetFamily.Application.FileProvider;
using PetFamily.Application.Providers;
using PetFamily.Domain.PetManagement.PetVO;
using PetFamily.Domain.PetManagement.VolunteerVO;
using PetFamily.Domain.Shared.ErrorContext;

namespace PetFamily.Application.Volunteers.Actions.Pets.Delete;

public class DeletePetPhotosHandler
{
    private const string BUCKET_NAME = "photos";
    
    private readonly IFileProvider _fileProvider;
    private readonly IVolunteersRepository _volunteersRepository;
    
    private Guid ExtractGuidFromPath(string path)
    {
        string fileName = Path.GetFileName(path);
        var match = Regex.Match(fileName, @"^([a-fA-F0-9\-]+)");

        return match.Success && Guid.TryParse(match.Value, out var id) ? id : Guid.Empty;
    }
    
    public DeletePetPhotosHandler(
        IFileProvider fileProvider,
        IVolunteersRepository volunteersRepository)
    {
        _fileProvider = fileProvider;
        _volunteersRepository = volunteersRepository;
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

        var photoIdsToDelete = command.PhotoIds;

        var transferFilesList = pet.TransferFilesList;

        List<PetPhoto> deletePhotos = [];
        List<ObjectName> objectNamesToDelete = [];
        
        foreach (var petPhoto in transferFilesList.Photos)
        {
            var petPhotoPath = petPhoto.PathToStorage.Path;
            
            var photoId = ExtractGuidFromPath(petPhotoPath);

            if(photoIdsToDelete.Contains(photoId))
            {
                objectNamesToDelete.Add(new ObjectName(petPhotoPath));
                deletePhotos.Add(petPhoto);
            }
        }
        
        var deleteFilesRequest = new CollectionsObjectName(objectNamesToDelete, BUCKET_NAME);
        
        var deleteResult = await _fileProvider.DeleteFiles(deleteFilesRequest, cancellationToken);
        if(deleteResult.IsFailure)
            return deleteResult.Error;
        
        pet.TransferFilesList.DeletePhotos(deletePhotos);

        await _volunteersRepository.Save(volunteerResult.Value, cancellationToken);
        
        return pet.Id.Value;
    }
}