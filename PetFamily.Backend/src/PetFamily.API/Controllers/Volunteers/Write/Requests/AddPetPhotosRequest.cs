namespace PetFamily.API.Controllers.Volunteers.Write.Requests;

public record AddPetPhotosRequest(
    IFormFileCollection Files);