using PetFamily.Application.Abstractions;

namespace PetFamily.Application.PetManagement.Commands.SpeciesСmd.DeleteBreed;

public record DeleteBreedCommand(
    Guid SpeciesId,
    Guid BreedId) : ICommand;