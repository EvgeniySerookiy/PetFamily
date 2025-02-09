using PetFamily.Domain.Shared;
using PetFamily.Domain.SharedVO;
using PetFamily.Domain.SpeciesContext.SpeciesVO;

namespace PetFamily.Domain.SpeciesContext.SpeciesEntities;

public class Species : Entity<SpeciesId>
{
    private readonly List<Breed> _breeds = new();
    public NotEmptyString Name { get; private set; }
    public IReadOnlyList<Breed> Breeds => _breeds;
    
    private Species(SpeciesId id) : base(id)
    {
    }

    private Species(SpeciesId id, NotEmptyString name, List<Breed> breeds) : base(id)
    {
        Name = name;
    }
    
    public void AddBreed(Breed breed)
    {
        _breeds.Add(breed);
    }

    public static Result<Species> Create(SpeciesId id, NotEmptyString name, List<Breed> breeds)
    {
        var createName = NotEmptyString.Create(name.Value);
        var species = new Species(id, createName.Value, breeds);
        
        return species;
    }
}