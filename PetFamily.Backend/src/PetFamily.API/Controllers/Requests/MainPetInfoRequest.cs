using PetFamily.Application.Volunteers.Actions.Pets.AddPet;
using PetFamily.Application.Volunteers.PetDTOs;
using PetFamily.Domain;

namespace PetFamily.API.Controllers.Requests;

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
    public MainPetInfoCommand ToCommand() => new MainPetInfoCommand(
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