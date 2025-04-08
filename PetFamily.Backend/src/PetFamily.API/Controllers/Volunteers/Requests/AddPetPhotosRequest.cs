namespace PetFamily.API.Controllers.Volunteers.Requests;

public record AddPetPhotosRequest(
    IFormFileCollection Files);