using PetFamily.Application.Volunteers.PetDTOs;

namespace PetFamily.API.Contracts;

public record AddPetPhotosRequest(
    IFormFileCollection Files);