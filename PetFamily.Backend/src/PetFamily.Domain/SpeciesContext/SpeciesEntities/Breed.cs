using PetFamily.Domain.Shared;
using PetFamily.Domain.SharedVO;
using PetFamily.Domain.SpeciesContext.SpeciesVO;

namespace PetFamily.Domain.SpeciesContext.SpeciesEntities;

public class Breed : Entity<BreedId>
{
    public NotEmptyString Name { get; private set; }
    
    private Breed(BreedId id) : base(id)
    {
    }

    private Breed(BreedId id, NotEmptyString name) : base(id)
    {
        Name = name;
    }

    public static Result<Breed> Create(BreedId id, NotEmptyString name)
    {
        var createName = NotEmptyString.Create(name.Value);

        var breed = new Breed(id, createName.Value);
        
        return breed;
    }
}