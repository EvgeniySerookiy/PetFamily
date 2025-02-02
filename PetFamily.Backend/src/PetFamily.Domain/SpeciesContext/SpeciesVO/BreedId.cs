namespace PetFamily.Domain.SpeciesContext.SpeciesVO;

public record BreedId
{
    public Guid Value { get; }

    private BreedId(Guid value)
    {
        Value = value;
    }

    public static BreedId Create(Guid value)
    {
        return new BreedId(value);
    }
    
    public static BreedId NewBreedId()
    {
        return new BreedId(Guid.NewGuid());
    }
    
    public static BreedId EmptyBreedId()
    {
        return new BreedId(Guid.Empty);
    }
}
