using CSharpFunctionalExtensions;

namespace PetFamily.Domain.PetVO;

public record Color
{
    public string Value { get; }

    private Color(string value)
    {
        Value = value;
    }

    public static Result<Color> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return Result.Failure<Color>("Color cannot be null or empty.");
        }

        var petColor = new Color(value);
        
        return Result.Success(petColor);
    }
}