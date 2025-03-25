namespace PetFamily.Application.Volunteers.Actions.Pets.DeletePetPhotos;

public record DeletePetPhotosCommand(
    Guid VolunteerId,
    Guid PetId,
    IEnumerable<Guid> PhotosId);