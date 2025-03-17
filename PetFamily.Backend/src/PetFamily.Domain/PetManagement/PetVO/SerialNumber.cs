using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared.ErrorContext;

namespace PetFamily.Domain.PetManagement.PetVO;

public record SerialNumber
{
    public static SerialNumber First = new(1);
    public int Value { get; }

    private SerialNumber(int value)
    {
        Value = value;
    }

    public static Result<SerialNumber, Error> Create(int value)
    {
        if(value <= 0)
            return Errors.General.ValueCannotBeNegative("Serial number", value);

        return new SerialNumber(value); 
    }
}