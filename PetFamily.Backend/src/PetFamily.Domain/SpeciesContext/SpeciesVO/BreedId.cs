namespace PetFamily.Domain.SpeciesContext.SpeciesVO;

public record BreedId
{
    public Guid Value { get; }
    
    private BreedId() { }

    private BreedId(Guid value)
    {
        Value = value;
    }
    
    public static BreedId Create(Guid id)
    {
        return new (id);
    }
    
    public static BreedId NewBreedId()
    {
        return new (Guid.NewGuid());
    }
    
    public static BreedId EmptyBreedId()
    {
        return new (Guid.Empty);
    }
}
