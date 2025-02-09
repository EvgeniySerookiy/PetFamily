using PetFamily.Domain.Shared;

namespace PetFamily.Domain.SharedVO;

public record PhoneNumber
{
    public const int MAX_PHONE_NUMBER_TEXT_LENGTH = 20;
    public string Value { get; }

    private PhoneNumber(string value)
    {
        Value = value;
    }

    public static Result<PhoneNumber> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return "Phone number cannot be empty.";
        }

        if (value.Length > MAX_PHONE_NUMBER_TEXT_LENGTH)
        {
            return $"Phone number cannot be longer than {MAX_PHONE_NUMBER_TEXT_LENGTH} characters.";
        }
        
        return new PhoneNumber(value);
    }
}