using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared.ErrorContext;
using PetFamily.Domain.SpeciesManagement.Entities;
using PetFamily.Domain.SpeciesManagement.SpeciesVO;

namespace PetFamily.Application.Database;

public interface ISpeciesWriteRepository
{
    Task<Guid> Add(
        Species species,
        CancellationToken cancellationToken = default);

    Task<Result<Guid, Error>> Delete(
        SpeciesId speciesId,
        CancellationToken cancellationToken = default);

    Task<Result<Species, Error>> GetById(
        SpeciesId speciesId,
        CancellationToken cancellationToken = default);
}