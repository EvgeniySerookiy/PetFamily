namespace PetFamily.Domain.SpeciesManagement.SpeciesVO;

public record SpeciesId
{
    public Guid Value { get; }
    private SpeciesId() { } 
    private SpeciesId(Guid value)
    {
        Value = value;
    }
    
    public static SpeciesId Create(Guid id) => new (id);

    public static SpeciesId NewSpeciesId() => new (Guid.NewGuid());
    
    public static SpeciesId EmptySpeciesId() => new (Guid.Empty);
}