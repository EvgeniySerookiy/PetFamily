using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared.ErrorContext;

namespace PetFamily.Domain.PetManagement.PetVO;

public record PetHealthInformation
{
    public const int MAX_HEALTH_INFORMATION_TEXT_LENGTH = 500;
    public string Value { get; }

    private PetHealthInformation(string value)
    {
        Value = value;
    }

    public static Result<PetHealthInformation, Error> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Errors.General.ValueIsRequired("Pet health information");
        
        if (value.Length > MAX_HEALTH_INFORMATION_TEXT_LENGTH)
            return Errors.General.ValueIsTooLong("Pet health information", MAX_HEALTH_INFORMATION_TEXT_LENGTH);
        
        return new PetHealthInformation(value);
    }
}