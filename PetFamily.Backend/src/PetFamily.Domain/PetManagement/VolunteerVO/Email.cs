using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared.ErrorContext;

namespace PetFamily.Domain.PetManagement.VolunteerVO;

public record Email
{
    public const int MAX_EMAIL_TEXT_LENGTH = 50;
    public string Value { get; }

    private Email(string value)
    {
        Value = value;
    }

    public static Result<Email, Error> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Errors.General.ValueIsRequired("Email");

        if (value.Length > MAX_EMAIL_TEXT_LENGTH)
            return Errors.General.ValueIsTooLong("Email", MAX_EMAIL_TEXT_LENGTH);
        
        return new Email(value);
    }
}