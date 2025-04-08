namespace PetFamily.API.Controllers.Volunteers.Write.Requests;

public record DeletePetPhotosRequest(
    IEnumerable<Guid> PhotosId);