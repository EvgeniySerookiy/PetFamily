using PetFamily.Application.Abstractions;

namespace PetFamily.Application.PetManagement.Commands.SpeciesСmd.DeleteSpecies;

public record DeleteSpeciesCommand(
    Guid SpeciesId) : ICommand;