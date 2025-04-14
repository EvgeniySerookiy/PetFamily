using PetFamily.Application.Abstractions;

namespace PetFamily.Application.PetManagement.Commands.SpeciesСmd.AddSpecies;

public record AddSpeciesCommand(
    string SpeciesName) : ICommand;