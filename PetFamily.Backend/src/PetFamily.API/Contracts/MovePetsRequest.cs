namespace PetFamily.API.Contracts;

public record MovePetsRequest(
    int CurrentPosition,
    int ToPosition);