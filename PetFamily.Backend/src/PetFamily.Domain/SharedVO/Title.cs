using PetFamily.Domain.Shared;

namespace PetFamily.Domain.SharedVO;

public record Title
{
    public const int MAX_TITLE_TEXT_LENGTH = 70;
    public string Value { get; }

    private Title(string value)
    {
        Value = value;
    }

    public static Result<Title> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return "Title cannot be empty.";
        }

        if (value.Length > MAX_TITLE_TEXT_LENGTH)
        {
            return $"Title cannot be longer than {MAX_TITLE_TEXT_LENGTH} characters.";
        }
        
        return new Title(value);
    }
}