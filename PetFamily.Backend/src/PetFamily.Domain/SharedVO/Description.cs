using PetFamily.Domain.Shared;

namespace PetFamily.Domain.SharedVO;

public record Description
{
    public const int MAX_DESCRIPTION_TEXT_LENGTH = 500;
    public string Value { get; }

    private Description(string value)
    {
        Value = value;
    }

    public static Result<Description> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return "Description cannot be empty.";
        }
        
        if (value.Length > MAX_DESCRIPTION_TEXT_LENGTH)
        {
            return $"Description cannot be longer than {MAX_DESCRIPTION_TEXT_LENGTH} characters.";
        }
        
        return new Description(value);
    }
}