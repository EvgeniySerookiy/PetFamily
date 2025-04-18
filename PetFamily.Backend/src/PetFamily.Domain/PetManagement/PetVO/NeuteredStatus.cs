using CSharpFunctionalExtensions;

namespace PetFamily.Domain.PetManagement.PetVO;

public record NeuteredStatus
{
    public bool Value { get; }

    private NeuteredStatus(bool value)
    {
        Value = value;
    }

    public static Result<NeuteredStatus> Create(bool value)
    {
        var neuteredStatus = new NeuteredStatus(value);
        
        return neuteredStatus;
    }
}