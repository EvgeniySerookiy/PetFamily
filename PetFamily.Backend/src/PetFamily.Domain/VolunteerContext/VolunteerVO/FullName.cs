using PetFamily.Domain.Shared;
using PetFamily.Domain.SharedVO;

namespace PetFamily.Domain.VolunteerContext.VolunteerVO;

public record FullName
{
    public const int MAX_MIDDLE_NAME_TEXT_LENGTH = 30;
    public NotEmptyString FirstName { get; }
    public NotEmptyString LastName { get; }
    public string? MiddleName { get; }
    
    private FullName() { }

    private FullName(NotEmptyString firstName, NotEmptyString lastName, string middleName)
    {
        FirstName = firstName;
        LastName = lastName;
        MiddleName = middleName;
    }

    public static Result<FullName> Create(NotEmptyString firstName, NotEmptyString lastName, string middleName)
    {
        if (middleName.Length > MAX_MIDDLE_NAME_TEXT_LENGTH)
        {
            return $"Middle name cannot be longer than {MAX_MIDDLE_NAME_TEXT_LENGTH} characters.";
        }
        
        var fullName = new FullName(firstName, lastName, middleName);
        
        return fullName;
    }
}