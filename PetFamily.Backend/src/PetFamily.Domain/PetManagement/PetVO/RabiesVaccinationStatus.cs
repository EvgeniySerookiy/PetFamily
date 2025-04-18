using CSharpFunctionalExtensions;

namespace PetFamily.Domain.PetManagement.PetVO;

public record RabiesVaccinationStatus
{
    public bool Value { get; }

    private RabiesVaccinationStatus(bool value)
    {
        Value = value;
    }

    public static Result<RabiesVaccinationStatus> Create(bool value)
    {
        var rabiesVaccinationStatus = new RabiesVaccinationStatus(value);
        
        return rabiesVaccinationStatus;
    }
}