using PetFamily.Application.PetManagement.Commands.Volunteers.MovePets;

namespace PetFamily.API.Controllers.Volunteers.Write.Requests;

public record MovePetsRequest(
    int CurrentPosition,
    int ToPosition)
{
    public MovePetsCommand ToCommand(Guid id) => new MovePetsCommand(id, CurrentPosition, ToPosition);
}