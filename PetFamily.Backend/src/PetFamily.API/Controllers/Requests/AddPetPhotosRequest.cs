namespace PetFamily.API.Controllers.Requests;

public record AddPetPhotosRequest(
    IFormFileCollection Files);