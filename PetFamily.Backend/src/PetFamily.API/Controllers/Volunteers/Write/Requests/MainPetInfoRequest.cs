using PetFamily.Application.Dtos.PetDTOs;
using PetFamily.Application.PetManagement.Commands.Volunteers.AddPet;
using PetFamily.Domain;

namespace PetFamily.API.Controllers.Volunteers.Write.Requests;

public record MainPetInfoRequest(
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
    DateTime DateOfCreation)
{
    public MainPetInfoCommand ToCommand(
        Guid volunteerId,
        Guid speciesId,
        Guid breedId
        ) => new MainPetInfoCommand(
        volunteerId,
        speciesId,
        breedId,
        Name,
        Title,
        Description,
        Color,
        PetHealthInformation,
        Address,
        PhoneNumber,
        PetSizeDto,
        IsNeutered,
        IsVaccinated,
        DateOfBirth,
        Status,
        DateOfCreation);
}