namespace PetFamily.Application.Volunteers.Actions.Pets.MovePets;

public record MovePetsCommand(
    Guid id,
    int CurrentPosition,
    int ToPosition);