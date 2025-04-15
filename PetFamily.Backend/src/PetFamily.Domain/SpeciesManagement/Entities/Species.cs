using CSharpFunctionalExtensions;
using PetFamily.Domain.SpeciesManagement.SpeciesVO;

namespace PetFamily.Domain.SpeciesManagement.Entities;

// Вид
public class Species : Shared.Entity<SpeciesId>
{
    private List<Breed> _breeds = new();
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

    public void AddBreed(Breed breed)
    {
        _breeds.Add(breed);
    }

    public static Result<Species> Create(SpeciesId id, SpeciesName speciesName, List<Breed> breeds)
    {
        var createName = SpeciesName.Create(speciesName.Value);

        var species = new Species(id, createName.Value, breeds);

        return species;
    }
}