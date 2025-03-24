using PetFamily.Domain;

namespace PetFamily.Application.Volunteers.PetDTOs;

public record MainPetInfoDto(
    string Name,
    string Title,
    string Description,
    string Color,
    string PetHealthInformation,
    PetAddressDto Address,
    string PhoneNumber,
    PetSize PetSize,
    bool IsNeutered,
    bool IsVaccinated,
    DateTime? DateOfBirth,
    AssistanceStatus Status,
    DateTime DateOfCreation);