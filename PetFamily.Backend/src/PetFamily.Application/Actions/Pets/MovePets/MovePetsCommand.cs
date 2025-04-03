namespace PetFamily.Application.Volunteers.Actions.Pets.MovePets;

public record MovePetsCommand(
    int CurrentPosition,
    int ToPosition);