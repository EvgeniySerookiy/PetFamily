namespace PetFamily.API.Contracts;

public record DeletePetPhotosRequest(
    List<Guid> PhotoIds);