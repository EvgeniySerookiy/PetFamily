using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared.ErrorContext;

namespace PetFamily.Application.Database;

public interface ISpeciesReadRepository
{
    Task<Result<Guid, Error>> GetSpeciesIdByName(
        string speciesName,
        CancellationToken cancelToken = default);

    Task<Result<Guid, Error>> CheckForTheBreedAndSpecies(
        Guid speciesId,
        Guid breedId,
        CancellationToken cancelToken = default);
}