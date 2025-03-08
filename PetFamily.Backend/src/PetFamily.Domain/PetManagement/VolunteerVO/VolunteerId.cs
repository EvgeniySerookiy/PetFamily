namespace PetFamily.Domain.PetManagement.VolunteerVO;

public record VolunteerId
{
    public Guid Value { get; }

    private VolunteerId(Guid value) => Value = value;
    
    public static VolunteerId Create(Guid id) => new (id);

    public static VolunteerId NewVolunteerId() => new (Guid.NewGuid());
    
    public static VolunteerId EmptyVolunteerId() => new (Guid.Empty);
    
    public static implicit operator Guid(VolunteerId breedId) => breedId.Value;

    public static implicit operator VolunteerId(Guid id) => new (id);
}