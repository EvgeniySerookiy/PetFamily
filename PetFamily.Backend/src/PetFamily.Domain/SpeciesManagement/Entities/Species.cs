using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared.ErrorContext;
using PetFamily.Domain.SpeciesManagement.SpeciesVO;

namespace PetFamily.Domain.SpeciesManagement.Entities;

// Вид
public class Species : Shared.Entity<SpeciesId>
{
    private List<Breed> _breeds = [];
    public SpeciesName SpeciesName { get; private set; }
    public IReadOnlyList<Breed> Breeds => _breeds;

    private Species(SpeciesId id) : base(id)
    {
    }

    private Species(SpeciesId id, SpeciesName speciesName, List<Breed> breeds) : base(id)
    {
        SpeciesName = speciesName;
        _breeds = breeds;
    }

    public void AddBreed(Breed breed) => _breeds.Add(breed);

    public UnitResult<Error> DeleteByBreedId(BreedId breedId)
    {
        var breed = _breeds.FirstOrDefault(b => b.Id == breedId);
        if (breed != null)
        {
            _breeds.Remove(breed);
            return UnitResult.Success<Error>();
        }
        
        return UnitResult.Failure(Errors.Breed.NotFound(breedId.Value));
    }

    public UnitResult<Error> EnsureBreedDoesNotExist(BreedName breedName)
    {
        var breedResult = _breeds
            .FirstOrDefault(b => b.BreedName == breedName);

        if (breedResult == null)
            return UnitResult.Success<Error>();

        return UnitResult.Failure(Errors.Breed.AlreadyExist());
    }

    public static Result<Species> Create(SpeciesId id, SpeciesName speciesName, List<Breed> breeds)
    {
        var createName = SpeciesName.Create(speciesName.Value);

        var species = new Species(id, createName.Value, breeds);

        return species;
    }
}