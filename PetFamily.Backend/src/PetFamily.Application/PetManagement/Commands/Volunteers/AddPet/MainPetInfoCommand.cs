using PetFamily.Application.Abstractions;
using PetFamily.Application.Dtos.PetDTOs;
using PetFamily.Domain;

namespace PetFamily.Application.PetManagement.Commands.Volunteers.AddPet;

public record MainPetInfoCommand(
    Guid VolunteerId,
    Guid SpeciesId,
    Guid BreedId,
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
    DateTime DateOfCreation) : ICommand;