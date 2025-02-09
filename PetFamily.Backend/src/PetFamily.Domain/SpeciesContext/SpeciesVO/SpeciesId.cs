namespace PetFamily.Domain.SpeciesContext.SpeciesVO;

public record SpeciesId
{
    public Guid Value { get; }
    private SpeciesId() { } 
    private SpeciesId(Guid value)
    {
        Value = value;
    }
    
    public static SpeciesId Create(Guid id)
    {
        return new (id);
    }
    
    public static SpeciesId NewSpeciesId()
    {
        return new (Guid.NewGuid());
    }
    
    public static SpeciesId EmptySpeciesId()
    {
        return new (Guid.Empty);
    }
}