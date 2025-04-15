using PetFamily.Application.Abstractions;

namespace PetFamily.Application.PetManagement.Commands.SpeciesСmd.AddBreed;

public record AddBreedCommand(
    Guid Id,
    string Name) : ICommand;