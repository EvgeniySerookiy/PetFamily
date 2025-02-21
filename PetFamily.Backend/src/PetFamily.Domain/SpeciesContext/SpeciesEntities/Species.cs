using CSharpFunctionalExtensions;
using PetFamily.Domain.PetContext.PetVO;
using PetFamily.Domain.SpeciesContext.SpeciesVO;

namespace PetFamily.Domain.SpeciesContext.SpeciesEntities;

public class Species : Shared.Entity<SpeciesId>
{
    private readonly List<Breed> _breeds = new();
    public PetName PetName { get; private set; }
    public IReadOnlyList<Breed> Breeds => _breeds;
    
    private Species(SpeciesId id) : base(id)
    {
    }

    private Species(SpeciesId id, PetName petName, List<Breed> breeds) : base(id)
    {
        PetName = petName;
    }
    
    public void AddBreed(Breed breed)
    {
        _breeds.Add(breed);
    }

    public static Result<Species> Create(SpeciesId id, PetName petName, List<Breed> breeds)
    {
        var createName = PetName.Create(petName.Value);
        
        var species = new Species(id, createName.Value, breeds);
        
        return species;
    }
}