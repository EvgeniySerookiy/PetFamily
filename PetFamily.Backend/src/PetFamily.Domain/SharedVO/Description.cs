using CSharpFunctionalExtensions;

namespace PetFamily.Domain.SharedVO;

public record Description
{
    public string Value { get; }

    private Description(string value)
    {
        Value = value;
    }

    public static Result<Description> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return Result.Failure<Description>("Description cannot be empty.");
        }

        var description = new Description(value);
        
        return Result.Success(description);
    }
}