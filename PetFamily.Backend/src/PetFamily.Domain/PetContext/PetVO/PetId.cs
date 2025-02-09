namespace PetFamily.Domain.PetContext.PetVO;

public record PetId
{
    public Guid Value { get; }

    private PetId(Guid value)
    {
        Value = value;
    }
    
    public static PetId Create(Guid id)
    {
        return new (id);
    }

    public static PetId NewPetId()
    { 
        return new (Guid.NewGuid());
    }
    
    public static PetId EmptyPetId()
    {
        return new (Guid.Empty);
    }
}