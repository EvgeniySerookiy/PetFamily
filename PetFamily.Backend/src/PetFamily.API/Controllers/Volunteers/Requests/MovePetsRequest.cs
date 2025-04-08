using PetFamily.Application.PetManagement.Commands.Pets.MovePets;

namespace PetFamily.API.Controllers.Volunteers.Requests;

public record MovePetsRequest(
    int CurrentPosition,
    int ToPosition)
{
    public MovePetsCommand ToCommand(Guid id) => new MovePetsCommand(id, CurrentPosition, ToPosition);
}