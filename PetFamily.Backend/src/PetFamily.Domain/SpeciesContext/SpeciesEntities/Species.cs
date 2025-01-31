using CSharpFunctionalExtensions;
using PetFamily.Domain.SharedVO;

namespace PetFamily.Domain.SpeciesContext.SpeciesEntities;

public class Species : Entity
{
    public Guid Id { get; private set; }
    public Name Name { get; private set; }
    public List<Breed> Breeds { get; private set; }

    private Species(Guid id, Name name, List<Breed> breeds)
    {
        Id = id;
        Name = name;
        Breeds = breeds;
    }

    public static Result<Species> Create(Guid id, Name name, List<Breed> breeds)
    {
        var createName = Name.Create(name.Value);
        if (createName.IsFailure)
        {
            return Result.Failure<Species>(createName.Error);
        }

        var species = new Species(id, createName.Value, breeds);
        
        return Result.Success(species);
    }
}