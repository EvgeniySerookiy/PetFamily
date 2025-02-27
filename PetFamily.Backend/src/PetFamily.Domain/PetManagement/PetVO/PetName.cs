using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared.ErrorContext;

namespace PetFamily.Domain.PetManagement.PetVO;

public record PetName
{
    public const int MAX_PET_NAME_TEXT_LENGTH = 100;
    public string Value { get; }

    private PetName(string value)
    {
        Value = value;
    }

    public static Result<PetName, Error> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Errors.General.ValueIsRequired("Pet name");
        
        if (value.Length > MAX_PET_NAME_TEXT_LENGTH)
            return Errors.General.ValueIsTooLong("Pet name", MAX_PET_NAME_TEXT_LENGTH);

        return new PetName(value);
    }
}