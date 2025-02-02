using CSharpFunctionalExtensions;

namespace PetFamily.Domain.SharedVO;

public record Name
{
    public string Value { get; }

    private Name(string value)
    {
        Value = value;
    }

    public static Result<Name> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return Result.Failure<Name>("Name cannot be empty");
        }
        
        var name = new Name(value);
        
        return Result.Success(name);
    }
}