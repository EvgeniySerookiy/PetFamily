namespace PetFamily.API.Controllers.Requests;

public record DeletePetPhotosRequest(
    IEnumerable<Guid> PhotosId);