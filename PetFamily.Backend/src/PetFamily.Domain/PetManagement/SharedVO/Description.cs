using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared.ErrorContext;

namespace PetFamily.Domain.PetManagement.SharedVO;

public record Description
{
    public const int MAX_DESCRIPTION_TEXT_LENGTH = 500;
    public string Value { get; }

    private Description(string value)
    {
        Value = value;
    }

    public static Result<Description, Error> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Errors.General.ValueIsRequired("Description");
        
        if (value.Length > MAX_DESCRIPTION_TEXT_LENGTH)
            return Errors.General.ValueIsTooLong("Description", MAX_DESCRIPTION_TEXT_LENGTH);
        
        return new Description(value);
    }
}