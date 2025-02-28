using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared.ErrorContext;

namespace PetFamily.Domain.PetManagement.SharedVO;

public record PhoneNumber
{
    public const int MAX_PHONE_NUMBER_TEXT_LENGTH = 20;
    public string Value { get; }

    private PhoneNumber(string value)
    {
        Value = value;
    }

    public static Result<PhoneNumber, Error> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Errors.General.ValueIsRequired("Phone number");

        if (value.Length > MAX_PHONE_NUMBER_TEXT_LENGTH)
            return Errors.General.ValueIsTooLong("Phone number", MAX_PHONE_NUMBER_TEXT_LENGTH);

        return new PhoneNumber(value);
    }
}