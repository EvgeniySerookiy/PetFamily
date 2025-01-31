using CSharpFunctionalExtensions;

namespace PetFamily.Domain.SharedVO;

public record PhoneNumber
{
    public string Value { get; }

    private PhoneNumber(string value)
    {
        Value = value;
    }

    public static Result<PhoneNumber> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return Result.Failure<PhoneNumber>("Phone number cannot be empty.");
        }

        var phoneNumber = new PhoneNumber(value);
        
        return Result.Success(phoneNumber);
    }
}