using PetFamily.Application.PetManagement.Commands.Volunteers.UpdatePet;
using PetFamily.Application.PetManagement.Commands.Volunteers.UpdatePetStatus;
using PetFamily.Domain;

namespace PetFamily.API.Controllers.Volunteers.Write.Requests;

public record UpdatePetStatusRequest(
    AssistanceStatus Status)
{
    public UpdatePetStatusCommand TCommand(
        Guid volunteerId,
        Guid petId) => 
        new UpdatePetStatusCommand(
            volunteerId, 
            petId, 
            Status);
}