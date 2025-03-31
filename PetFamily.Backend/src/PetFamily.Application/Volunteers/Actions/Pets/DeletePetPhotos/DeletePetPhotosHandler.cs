using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Database;
using PetFamily.Application.Extensions;
using PetFamily.Application.Photos;
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
    private readonly IValidator<DeletePetPhotosCommand> _validator;
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly ILogger<AddPetHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    
    public DeletePetPhotosHandler(
        IFileProvider fileProvider,
        IValidator<DeletePetPhotosCommand> validator,
        IVolunteersRepository volunteersRepository,
        ILogger<AddPetHandler> logger,
        IUnitOfWork unitOfWork)
    {
        _fileProvider = fileProvider;
        _validator = validator;
        _volunteersRepository = volunteersRepository;
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        Guid volunteerId,
        Guid petId,
        DeletePetPhotosCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToErrorList();
        
        var volunteerResult = await _volunteersRepository.GetById(
            VolunteerId.Create(volunteerId),
            cancellationToken);
        
        var pet = volunteerResult.Value.Pets
            .FirstOrDefault(p => p.Id == petId);
        
        if(pet == null)
            return Errors.General.NotFound(petId).ToErrorList();

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
        
        var deleteResult = await _fileProvider.DeletePhotos(photosPathWithBucket, cancellationToken);
        if(deleteResult.IsFailure)
            return deleteResult.Error.ToErrorList();

        pet.PetPhotos.RemoveAll(photos);
        
        await _unitOfWork.SaveChanges(cancellationToken);
        
        _logger.LogInformation("Deleted photos to pet with id {PetId} from a volunteer with id {VolunteerId}",
            petId, volunteerId);
        
        return pet.Id.Value;
    }
    
    private Guid ExtractGuidFromPath(string path)
    {
        string fileName = Path.GetFileName(path);
        var match = Regex.Match(fileName, @"^([a-fA-F0-9\-]+)");

        return match.Success && Guid.TryParse(match.Value, out var id) ? id : Guid.Empty;
    }
}