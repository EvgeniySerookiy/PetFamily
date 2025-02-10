namespace PetFamily.Domain.VolunteerContext.VolunteerVO;

public record VolunteerId
{
    public Guid Value { get; }

    private VolunteerId(Guid value)
    {
        Value = value;
    }
    
    public static VolunteerId Create(Guid id)
    {
        return new (id);
    }

    public static VolunteerId NewVolunteerId()
    {
        return new (Guid.NewGuid());
    }
    
    public static VolunteerId EmptyVolunteerId()
    {
        return new (Guid.Empty);
    }
}