using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared.ErrorContext;
using PetFamily.Domain.SpeciesManagement.SpeciesVO;

namespace PetFamily.Domain.SpeciesManagement.Entities;

// Порода
public class Breed : Shared.Entity<BreedId>
{
    public BreedName BreedName { get; private set; }
    
    private Breed(BreedId id) : base(id) {}

    private Breed(BreedId id, BreedName breedName) : base(id)
    {
        BreedName = breedName;
    }

    public static Result<Breed, Error> Create(BreedId id, BreedName breedName)
    {
        var createName = BreedName.Create(breedName.Value);

        var breed = new Breed(id, createName.Value);
        
        return breed;
    }
}