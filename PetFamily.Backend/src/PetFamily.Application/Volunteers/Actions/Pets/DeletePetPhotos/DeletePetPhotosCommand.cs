namespace PetFamily.Application.Volunteers.Actions.Pets.DeletePetPhotos;

public record DeletePetPhotosCommand(
    IEnumerable<Guid> PhotosId);