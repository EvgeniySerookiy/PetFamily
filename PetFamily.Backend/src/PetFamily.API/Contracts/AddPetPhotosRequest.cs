namespace PetFamily.API.Contracts;

public record AddPetPhotosRequest(
    IFormFileCollection Files);