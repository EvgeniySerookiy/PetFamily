namespace PetFamily.Application.Volunteers.Actions.Pets.Delete;

public record DeletePetPhotosCommand(
    Guid VolunteerId,
    Guid PetId,
    List<Guid> PhotoIds);