using PetFamily.Application.Volunteers.PetDTOs;
using PetFamily.Domain;

namespace PetFamily.Application.Volunteers.Actions.Pets.AddPet;

public record MainPetInfoCommand(
    string Name,
    string Title,
    string Description,
    string Color,
    string PetHealthInformation,
    PetAddressDto Address,
    string PhoneNumber,
    PetSizeDto PetSizeDto,
    bool IsNeutered,
    bool IsVaccinated,
    DateTime? DateOfBirth,
    AssistanceStatus Status,
    DateTime DateOfCreation);