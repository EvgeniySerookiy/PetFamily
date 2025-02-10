using PetFamily.Domain.Shared;

namespace PetFamily.Domain.PetContext.PetVO;

public record PetHealthInformation
{
    public const int MAX_HEALTH_INFORMATION_TEXT_LENGTH = 500;
    public string Value { get; }

    private PetHealthInformation(string value)
    {
        Value = value;
    }

    public static Result<PetHealthInformation> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return "Pet health information cannot be null or empty.";
        }
        
        if (value.Length > MAX_HEALTH_INFORMATION_TEXT_LENGTH)
        {
            return $"Pet health information be longer than {MAX_HEALTH_INFORMATION_TEXT_LENGTH} characters.";
        }
        
        return new PetHealthInformation(value);
    }
}