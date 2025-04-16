using PetFamily.Application.Dtos.PetDTOs;
using PetFamily.Application.PetManagement.Commands.Volunteers.UpdatePet;
using PetFamily.Domain;
using PetFamily.Domain.PetManagement.PetVO;
using PetFamily.Domain.PetManagement.VolunteerVO;

namespace PetFamily.API.Controllers.Volunteers.Write.Requests;

public record UpdatePetRequest(
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
    DateTime DateOfCreation)
{
    public UpdatePetCommand ToCommand(
        Guid volunteerId,
        Guid petId) =>
        new UpdatePetCommand(
            volunteerId,
            petId,
            SpeciesId,
            BreedId,
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