namespace PetFamily.Domain.SpeciesContext.SpeciesVO;

public record BreedId
{
    public Guid Value { get; }
    
    private BreedId() { }

    private BreedId(Guid value)
    {
        Value = value;
    }
    
    public static BreedId Create(Guid id) => new (id);
    
    public static BreedId NewBreedId() => new (Guid.NewGuid());
    
    public static BreedId EmptyBreedId() => new (Guid.Empty);
}
