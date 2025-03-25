namespace PetFamily.API.Contracts;

public record DeletePetPhotosRequest(
    IEnumerable<Guid> PhotosId);