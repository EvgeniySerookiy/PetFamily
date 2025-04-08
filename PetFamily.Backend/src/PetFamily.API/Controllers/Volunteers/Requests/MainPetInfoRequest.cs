using PetFamily.Application.Dtos.PetDTOs;
using PetFamily.Application.PetManagement.Commands.Pets.AddPet;
using PetFamily.Domain;

namespace PetFamily.API.Controllers.Volunteers.Requests;

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
    public MainPetInfoCommand ToCommand(Guid id) => new MainPetInfoCommand(
        id,
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