using PetFamily.Domain.Shared;

namespace PetFamily.Domain.SharedVO;

public record NotEmptyString
{
    public const int MAX_NOT_EMPTY_STRING_TEXT_LENGTH = 30;
    public string Value { get; }
    
    private NotEmptyString(string value)
    {
        Value = value;
    }

    public static Result<NotEmptyString> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return "String cannot be empty";
        }

        if (value.Length > MAX_NOT_EMPTY_STRING_TEXT_LENGTH)
        {
            return $"String cannot be longer than {MAX_NOT_EMPTY_STRING_TEXT_LENGTH} characters.";
        }

        return new NotEmptyString(value);
    }
    
}