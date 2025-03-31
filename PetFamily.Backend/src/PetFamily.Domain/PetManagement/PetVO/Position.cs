using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared.ErrorContext;

namespace PetFamily.Domain.PetManagement.PetVO;

public record Position
{
    public static Position First = new(1);
    public int Value { get; }

    private Position(int value)
    {
        Value = value;
    }

    public static Result<Position, Error> Create(int value)
    {
        if(value <= 0)
            return Errors.General.ValueCannotBeNegative("Position", value);

        return new Position(value); 
    }
}