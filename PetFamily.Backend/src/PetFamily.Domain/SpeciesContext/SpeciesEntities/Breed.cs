using CSharpFunctionalExtensions;
using PetFamily.Domain.SharedVO;

namespace PetFamily.Domain.SpeciesContext.SpeciesEntities;

public class Breed : Entity
{
    public Guid Id { get; private set; }
    public Name Name { get; private set; }

    private Breed(Guid id, Name name)
    {
        Id = id;
        Name = name;
    }

    public static Result<Breed> Create(Guid id, Name name)
    {
        var createName = Name.Create(name.Value);
        if (createName.IsFailure)
        {
            return Result.Failure<Breed>(createName.Error);
        }

        var breed = new Breed(id, createName.Value);
        
        return Result.Success(breed);
    }
}