using CSharpFunctionalExtensions;
using PetFamily.Domain.PetContext;
using PetFamily.Domain.SharedVO;

namespace PetFamily.Domain.SpeciesContext.SpeciesEntities;

public class Species : Entity
{
    private readonly List<Breed> _breeds = new();
    public Guid Id { get; private set; }
    public Name Name { get; private set; }
    public IReadOnlyList<Breed> Breeds => _breeds;

    private Species(Guid id, Name name, List<Breed> breeds)
    {
        Id = id;
        Name = name;
    }
    
    public void AddBreed(Breed breed)
    {
        _breeds.Add(breed);
    }

    public static Result<Species> Create(Guid id, Name name, List<Breed> breeds)
    {
        var createName = Name.Create(name.Value);
        var species = new Species(id, createName.Value, breeds);
        
        return Result.Success(species);
    }
}