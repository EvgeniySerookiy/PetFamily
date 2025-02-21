using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared.ErrorContext;

namespace PetFamily.Domain.SharedVO;

public record Title
{
    public const int MAX_TITLE_TEXT_LENGTH = 70;
    public string Value { get; }

    private Title(string value)
    {
        Value = value;
    }

    public static Result<Title, Error> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Errors.General.ValueIsRequired("Title");
        
        if (value.Length > MAX_TITLE_TEXT_LENGTH)
            return Errors.General.ValueIsTooLong("Title", MAX_TITLE_TEXT_LENGTH);
        
        return new Title(value);
    }
}