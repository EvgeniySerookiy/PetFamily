using CSharpFunctionalExtensions;

namespace PetFamily.Domain.SharedVO;

public record Title
{
    public string Value { get; }

    private Title(string value)
    {
        Value = value;
    }

    public static Result<Title> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return Result.Failure<Title>("Title is required.");
        }

        var title = new Title(value);
        
        return Result.Success(title);
    }
}