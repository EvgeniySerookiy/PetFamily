using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared.ErrorContext;

namespace PetFamily.Domain.VolunteerContext.VolunteerVO;

public record FullName
{
    public const int MAX_ALL_NAME_TEXT_LENGTH = 50;
    public string FirstName { get; }
    public string LastName { get; }
    public string? MiddleName { get; }

    private FullName(string firstName, string lastName, string middleName)
    {
        FirstName = firstName;
        LastName = lastName;
        MiddleName = middleName;
    }

    public static Result<FullName, Error> Create(
        string firstName, 
        string lastName, 
        string middleName)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            return Errors.General.ValueIsRequired("First name");
        
        if (string.IsNullOrWhiteSpace(lastName))
            return Errors.General.ValueIsRequired("Last name");
        
        if (firstName.Length > MAX_ALL_NAME_TEXT_LENGTH)
            return Errors.General.ValueIsTooLong("First name", MAX_ALL_NAME_TEXT_LENGTH);
        
        if (lastName.Length > MAX_ALL_NAME_TEXT_LENGTH)
            return Errors.General.ValueIsTooLong("Last name", MAX_ALL_NAME_TEXT_LENGTH);
        
        if (middleName.Length > MAX_ALL_NAME_TEXT_LENGTH)
            return Errors.General.ValueIsTooLong("Middle name", MAX_ALL_NAME_TEXT_LENGTH);
        
        return new FullName(firstName, lastName, middleName);
    }
}