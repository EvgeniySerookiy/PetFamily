namespace PetFamily.Domain.PetContext.PetVO;

public record PetId
{
    public Guid Value { get; }

    private PetId(Guid value) => Value = value;

    public static PetId Create(Guid id) => new(id);

    public static PetId NewPetId() => new (Guid.NewGuid());
    
    public static PetId EmptyPetId() => new (Guid.Empty);
}