using CSharpFunctionalExtensions;
using PetFamily.Domain.PetManagement.PetVO;
using PetFamily.Domain.SpesiesManagment.SpeciesVO;

namespace PetFamily.Domain.SpesiesManagment.Entities;

public class Breed : Shared.Entity<BreedId>
{
    public PetName PetName { get; private set; }
    
    private Breed(BreedId id) : base(id) {}

    private Breed(BreedId id, PetName petName) : base(id)
    {
        PetName = petName;
    }

    public static Result<Breed> Create(BreedId id, PetName petName)
    {
        var createName = PetName.Create(petName.Value);

        var breed = new Breed(id, createName.Value);
        
        return breed;
    }
}