using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetFamily.Application.Database;
using PetFamily.Application.Dtos;
using PetFamily.Domain.Shared.ErrorContext;
using PetFamily.Domain.SpeciesManagement.Entities;
using PetFamily.Domain.SpeciesManagement.SpeciesVO;
using PetFamily.Infrastructure.DbContexts;

namespace PetFamily.Infrastructure.Repositories;

public class SpeciesRepository : ISpeciesRepository
{
    private readonly WriteDbContext _writeDbContext;
    private readonly IUnitOfWork _unitOfWork;

    public SpeciesRepository(
        WriteDbContext writeDbContext,
        IUnitOfWork unitOfWork)
    {
        _writeDbContext = writeDbContext;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Add(
        Species species,
        CancellationToken cancellationToken = default)
    {
        await _writeDbContext.Species.AddAsync(species, cancellationToken);
        await _unitOfWork.SaveChanges(cancellationToken);

        return species.Id.Value;
    }
    
    public async Task<Guid> DeleteSpecies(
        SpeciesId speciesId, 
        CancellationToken cancellationToken = default)
    {
        var speciesToDelete = await _writeDbContext.Species
            .FirstOrDefaultAsync(s => s.Id == speciesId, cancellationToken);
        
        _writeDbContext.Species.Remove(speciesToDelete);
        
        return speciesToDelete.Id.Value;
    }
    
    public async Task<Guid> DeleteBreed(
        BreedId breedId, 
        CancellationToken cancellationToken = default)
    {
        var breedToDelete = await _writeDbContext.Breeds
            .FirstOrDefaultAsync(s => s.Id == breedId, cancellationToken);
        
        _writeDbContext.Breeds.Remove(breedToDelete);
        
        return breedToDelete.Id.Value;
    }

    public async Task<Result<Species, Error>> GetById(
        SpeciesId speciesId,
        CancellationToken cancellationToken = default)
    {
        var species = await _writeDbContext.Species
            .Include(v => v.Breeds)
            .FirstOrDefaultAsync(v => v.Id == speciesId, cancellationToken);

        if (species is null)
            return Errors.General.NotFound(speciesId.Value);

        return species;
    }

    public async Task<Result<Species, Error>> GetBySpeciesName(
        SpeciesName speciesName,
        CancellationToken cancellationToken = default)
    {
        var species = await _writeDbContext.Species
            .FirstOrDefaultAsync(v => v.SpeciesName == speciesName, cancellationToken);

        if (species is null)
            return Errors.General.NotFound();

        return species;
    }

    public async Task<Result<Species, Error>> GetByBreedName(
        SpeciesId speciesId,
        BreedName breedName,
        CancellationToken cancellationToken = default)
    {
        var species = await _writeDbContext.Species
            .Include(v => v.Breeds)
            .FirstOrDefaultAsync(v => v.Id == speciesId, cancellationToken);
        if (species is null)
            return Errors.General.NotFound();

        var result = species.Breeds
            .Any(b => b.BreedName.Value == breedName.Value);
        if (result is false)
            return species;

        return Errors.Breed.AlreadyExist();
    }
}