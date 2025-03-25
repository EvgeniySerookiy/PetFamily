namespace PetFamily.Application.Volunteers.Actions.Pets.MovePets;

public record MovePetsCommand(
    Guid VolunteerId,
    Guid PetIdMove,
    Guid PetIdTarget);