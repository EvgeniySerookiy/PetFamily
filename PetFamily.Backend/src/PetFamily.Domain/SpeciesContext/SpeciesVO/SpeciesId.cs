namespace PetFamily.Domain.SpeciesContext.SpeciesVO;

public record SpeciesId
{
    public Guid Value { get; }

    private SpeciesId(Guid value)
    {
        Value = value;
    }

    public static SpeciesId Create(Guid value)
    {
        return new SpeciesId(value);
    }
    
    public static SpeciesId NewSpeciesId()
    {
        return new SpeciesId(Guid.NewGuid());
    }
    
    public static SpeciesId EmptySpeciesId()
    {
        return new SpeciesId(Guid.Empty);
    }
}