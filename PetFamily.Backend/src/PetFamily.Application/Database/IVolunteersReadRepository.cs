using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared.ErrorContext;

namespace PetFamily.Application.Database;

public interface IVolunteersReadRepository
{
    Task<Result<Guid, Error>> IsSpeciesUsedByAnyPet(
        Guid speciesId,
        CancellationToken cancelToken = default);

    Task<Result<Guid, Error>> IsBreedUsedByAnyPet(
        Guid speciesId,
        Guid breedId,
        CancellationToken cancelToken = default);
    
    Task<Result<Guid, Error>> IsEmailAlreadyRegistered(
        string email,
        CancellationToken cancelToken = default);

    Task<Result<Guid, Error>> CheckWithVolunteerFoarAPet(
        Guid volunteerId,
        Guid petId,
        CancellationToken cancelToken = default);
}