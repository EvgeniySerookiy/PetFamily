using CSharpFunctionalExtensions;

namespace PetFamily.Domain.PetVO;

public record PetHealthInformation
{
    public string Value { get; }

    private PetHealthInformation(string value)
    {
        Value = value;
    }

    public static Result<PetHealthInformation> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return Result.Failure<PetHealthInformation>("Pet health information cannot be null or empty.");
        }

        var petHealthInformation = new PetHealthInformation(value);
        
        return Result.Success(petHealthInformation);
    }
}