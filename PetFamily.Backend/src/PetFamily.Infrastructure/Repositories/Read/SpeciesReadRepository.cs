using System.Data;
using CSharpFunctionalExtensions;
using Dapper;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Database;
using PetFamily.Domain.Shared.ErrorContext;
using PetFamily.Domain.SpeciesManagement.Entities;

namespace PetFamily.Infrastructure.Repositories.Read;

public class SpeciesReadRepository : ISpeciesReadRepository
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;
    private readonly ILogger<SpeciesReadRepository> _logger;

    public SpeciesReadRepository(
        ISqlConnectionFactory sqlConnectionFactory,
        ILogger<SpeciesReadRepository> logger)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
        _logger = logger;
    }

    public async Task<Result<Guid, Error>> GetSpeciesIdByName(
        string speciesName,
        CancellationToken cancelToken = default)
    {
        var connection = _sqlConnectionFactory.Create();

        var parameters = new DynamicParameters();
        parameters.Add("@SpeciesName", $"%{speciesName}%");

        var speciesId = await connection.ExecuteScalarAsync<Guid?>(
            @"SELECT id FROM species 
                  WHERE LOWER(species_name) LIKE LOWER(@SpeciesName)", 
            parameters);

        if (speciesId == null)
        {
            _logger.LogWarning(
                "Species not found with name: {SpeciesName}", 
                speciesName);
            return Errors.Species.NotFoundByName(speciesName);
        }

        _logger.LogInformation(
            "Species found name: {SpeciesName}, id: {SpeciesId}", 
            speciesName, speciesId.Value);
        
        return speciesId.Value;
    }

    public async Task<Result<Guid, Error>> CheckForTheBreedAndSpecies(
        Guid speciesId,
        Guid breedId,
        CancellationToken cancelToken = default)
    {
        var connection = _sqlConnectionFactory.Create();

        var parameters = new DynamicParameters();
        parameters.Add("@SpeciesId", speciesId);
        parameters.Add("@BreedId", breedId);
        
        var breedResult = await connection.ExecuteScalarAsync<Guid?>(
            @"SELECT id FROM breeds 
                  WHERE species_id = @SpeciesId
                  AND id = @BreedId
                  LIMIT 1", 
            parameters);

        if (breedResult == null)
        {
            _logger.LogWarning(
                "Breed with id: {BreedId} not found for Species with id: {SpeciesId}",
                breedId, speciesId);
            
            return Errors.Breed.NotFound(breedId);
        }
        
        _logger.LogInformation(
            "Successfully found Breed with id: {BreedId} for Species with id: {SpeciesId}.",
           breedId, speciesId);

        return breedId;
    }
    
}