using PetFamily.Application.Volunteers.DTOs.Collections;
using PetFamily.Application.Volunteers.PetDTOs;

namespace PetFamily.API.Contracts;

public record AddPetRequest(
    MainPetInfoDto MainPetInfo,
    IFormFileCollection Files);