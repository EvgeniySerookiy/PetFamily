using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared.ErrorContext;
using PetFamily.Domain.SpeciesManagement.Entities;
using PetFamily.Domain.SpeciesManagement.SpeciesVO;

namespace PetFamily.Application.Database;

public interface ISpeciesRepository
{
    Task<Guid> Add(Species species, CancellationToken cancellationToken = default);

    Task<Result<Species, Error>> GetById(
        SpeciesId speciesId,
        CancellationToken cancellationToken = default);

    Task<Result<Guid, Error>> DeleteSpecies(
        SpeciesId speciesId,
        CancellationToken cancellationToken = default);

    Task<Result<Guid, Error>> DeleteBreed(
        BreedId breedId,
        CancellationToken cancellationToken = default);

    Task<Result<Species, Error>> GetBySpeciesName(
        SpeciesName speciesName,
        CancellationToken cancellationToken = default);

    // Task<Result<Species, Error>> GetByBreedName(
    //     SpeciesId speciesId,
    //     BreedName breedName,
    //     CancellationToken cancellationToken = default);
}