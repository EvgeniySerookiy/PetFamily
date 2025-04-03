using PetFamily.Application.Volunteers.Actions.Pets.MovePets;

namespace PetFamily.API.Controllers.Requests;

public record MovePetsRequest(
    int CurrentPosition,
    int ToPosition)
{
    public MovePetsCommand ToCommand() => new MovePetsCommand(CurrentPosition, ToPosition);
}