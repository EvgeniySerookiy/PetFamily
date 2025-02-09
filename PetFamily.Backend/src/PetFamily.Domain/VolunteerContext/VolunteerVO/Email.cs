using PetFamily.Domain.Shared;

namespace PetFamily.Domain.VolunteerContext.VolunteerVO;

public record Email
{
    public const int MAX_EMAIL_TEXT_LENGTH = 50;
    public string Value { get; }

    private Email(string value)
    {
        Value = value;
    }

    public static Result<Email> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return "Email cannot be empty.";
        }

        if (value.Length > MAX_EMAIL_TEXT_LENGTH)
        {
            return $"Email cannot be longer than {MAX_EMAIL_TEXT_LENGTH} characters.";
        }
        
        return new Email(value);
    }
}