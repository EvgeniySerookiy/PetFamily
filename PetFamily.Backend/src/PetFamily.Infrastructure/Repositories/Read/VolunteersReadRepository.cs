using CSharpFunctionalExtensions;
using Dapper;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Database;
using PetFamily.Domain.Shared.ErrorContext;

namespace PetFamily.Infrastructure.Repositories.Read;

public class VolunteersReadRepository : IVolunteersReadRepository
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;
    private readonly ILogger<VolunteersReadRepository> _logger;

    public VolunteersReadRepository(
        ISqlConnectionFactory sqlConnectionFactory,
        ILogger<VolunteersReadRepository> logger)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
        _logger = logger;
    }

    public async Task<Result<Guid, Error>> IsSpeciesUsedByAnyPet(
        Guid speciesId,
        CancellationToken cancelToken = default)
    {
        var connection = _sqlConnectionFactory.Create();

        var parameters = new DynamicParameters();
        parameters.Add("@SpeciesId", speciesId);

        var petId = await connection.ExecuteScalarAsync<Guid?>(
            @"SELECT id FROM pets 
                  WHERE species_id = @SpeciesId
                  LIMIT 1",
            parameters);

        if (petId != null)
        {
            _logger.LogWarning(
                "Pet found with speciesId: {SpeciesId}, petId: {PetId}. " +
                "Cannot proceed with the operation",
                speciesId, petId.Value);

            return Errors.Species.AlreadyExist();
        }

        _logger.LogInformation(
            "No pet found with speciesId: {SpeciesId}. " +
            "Proceeding with the operation",
            speciesId);

        return speciesId;
    }

    public async Task<Result<Guid, Error>> IsBreedUsedByAnyPet(
        Guid speciesId,
        Guid breedId,
        CancellationToken cancelToken = default)
    {
        var connection = _sqlConnectionFactory.Create();

        var parameters = new DynamicParameters();
        parameters.Add("@SpeciesId", speciesId);
        parameters.Add("@BreedId", breedId);

        var petId = await connection.ExecuteScalarAsync<Guid?>(
            @"SELECT id FROM pets 
                  WHERE species_id = @SpeciesId
                  AND breed_id = @BreedId
                  LIMIT 1",
            parameters);

        if (petId != null)
        {
            _logger.LogWarning(
                "Breed with id: {BreedId} (SpeciesId: {SpeciesId}) is currently used by one or more pets.",
                breedId, speciesId);

            return Errors.Breed.AlreadyExist();
        }

        _logger.LogInformation(
            "Breed with id: {BreedId} (SpeciesId: {SpeciesId}) is not used by any pets.",
            breedId, speciesId);

        return breedId;
    }

    public async Task<Result<Guid, Error>> IsEmailAlreadyRegistered(
        string email,
        CancellationToken cancelToken = default)
    {
        var connection = _sqlConnectionFactory.Create();

        var parameters = new DynamicParameters();
        parameters.Add("@Email", email);

        var volunteerId = await connection.ExecuteScalarAsync<Guid?>(
            @"SELECT id FROM volunteers 
                  WHERE email = @Email
                  LIMIT 1",
            parameters);

        if (volunteerId == null)
        {
            _logger.LogWarning(
                "No volunteer found with email: {Email}",
                email);
            return Errors.Volunteer.AlreadyExist();
        }

        _logger.LogInformation(
            "Volunteer found with email: {Email}, id: {VolunteerId}",
            email, volunteerId.Value);

        return volunteerId.Value;
    }

    public async Task<Result<Guid, Error>> CheckWithVolunteerFoarAPet(
        Guid volunteerId,
        Guid petId,
        CancellationToken cancelToken = default)
    {
        var connection = _sqlConnectionFactory.Create();

        var parameters = new DynamicParameters();
        parameters.Add("@VolunteerId", volunteerId);
        parameters.Add("@PetId", petId);

        var pet = await connection.ExecuteScalarAsync<Guid?>(
            @"SELECT id FROM pets 
                  WHERE volunteer_id = @VolunteerId
                  AND id = @PetId
                  LIMIT 1",
            parameters);

        if (pet == null)
        {
            _logger.LogWarning(
                "Pet with id: {PetId} not found for Volunteer with id: {VolunteerId}",
                petId, volunteerId);

            return Errors.Pet.NotFound(petId);
        }

        _logger.LogInformation(
            "Pet with id {PetId} found for Volunteer with id {VolunteerId}",
            petId, volunteerId);

        return petId;
    }
}