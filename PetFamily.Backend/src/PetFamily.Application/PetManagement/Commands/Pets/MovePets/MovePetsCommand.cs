using PetFamily.Application.Abstractions;

namespace PetFamily.Application.PetManagement.Commands.Pets.MovePets;

public record MovePetsCommand(
    Guid Id,
    int CurrentPosition,
    int ToPosition) : ICommand;