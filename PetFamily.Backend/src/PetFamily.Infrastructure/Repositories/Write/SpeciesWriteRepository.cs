using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Database;
using PetFamily.Domain.Shared.ErrorContext;
using PetFamily.Domain.SpeciesManagement.Entities;
using PetFamily.Domain.SpeciesManagement.SpeciesVO;
using PetFamily.Infrastructure.DbContexts;

namespace PetFamily.Infrastructure.Repositories.Write;

public class SpeciesWriteRepository : ISpeciesWriteRepository
{
    private readonly WriteDbContext _writeDbContext;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<SpeciesWriteRepository> _logger;

    public SpeciesWriteRepository(
        WriteDbContext writeDbContext,
        IUnitOfWork unitOfWork,
        ILogger<SpeciesWriteRepository> logger)
    {
        _writeDbContext = writeDbContext;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Guid> Add(
        Species species,
        CancellationToken cancellationToken = default)
    {
        await _writeDbContext.Species.AddAsync(species, cancellationToken);
        await _unitOfWork.SaveChanges(cancellationToken);
        
        _logger.LogInformation("Species with id: {SpeciesId} it was successfully added", species.Id.Value);

        return species.Id.Value;
    }
    
    public async Task<Result<Guid, Error>> Delete(
        SpeciesId speciesId, 
        CancellationToken cancellationToken = default)
    {
        var speciesToDelete = await _writeDbContext.Species
            .FirstOrDefaultAsync(s => s.Id == speciesId, cancellationToken);

        if (speciesToDelete == null)
        {
            _logger.LogWarning("Attempted to delete non-existent species with id: {SpeciesId}", speciesId);
            return Errors.Species.NotFound(speciesId.Value);
        }
        
        _writeDbContext.Species.Remove(speciesToDelete);
        
        await _unitOfWork.SaveChanges(cancellationToken);
        
        _logger.LogInformation("Successfully deleted species with id: {SpeciesId}", speciesId);
        
        return speciesToDelete.Id.Value;
    }
    

    public async Task<Result<Species, Error>> GetById(
        SpeciesId speciesId,
        CancellationToken cancellationToken = default)
    {
        var species = await _writeDbContext.Species
            .Include(s => s.Breeds)
            .FirstOrDefaultAsync(v => v.Id == speciesId, cancellationToken);

        if (species == null)
        {
            _logger.LogWarning("Attempted to delete non-existent species with id: {SpeciesId}", speciesId);
            return Errors.Species.NotFound(speciesId.Value);
        }
        
        _logger.LogInformation("Successfully retrieved species with id: {SpeciesId}", speciesId);

        return species;
    }
    
}