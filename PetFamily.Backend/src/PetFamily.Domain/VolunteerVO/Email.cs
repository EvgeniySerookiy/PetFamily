using CSharpFunctionalExtensions;

namespace PetFamily.Domain.VolunteerVO;

public record Email
{
    public string Value { get; }

    private Email(string value)
    {
        Value = value;
    }

    public static Result<Email> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return Result.Failure<Email>("Email cannot be empty.");
        }

        var email = new Email(value);
        
        return Result.Success(email);
    }
}