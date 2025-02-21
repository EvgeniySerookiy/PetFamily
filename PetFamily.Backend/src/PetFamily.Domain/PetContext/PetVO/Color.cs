using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared.ErrorContext;

namespace PetFamily.Domain.PetContext.PetVO;

public record Color
{
    public const int MAX_COLOR_TEXT_LENGTH = 100;
    public string Value { get; }

    private Color(string value)
    {
        Value = value;
    }

    public static Result<Color, Error> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Errors.General.ValueIsRequired("Color");
        
        if (value.Length > MAX_COLOR_TEXT_LENGTH)
            return Errors.General.ValueIsTooLong("Color", MAX_COLOR_TEXT_LENGTH);

        return new Color(value);
    }
}