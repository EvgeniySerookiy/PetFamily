using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared.ErrorContext;

namespace PetFamily.Domain.VolunteerContext.VolunteerVO;

public record YearsOfExperience
{
    public int Value { get; }

    private YearsOfExperience(int value)
    {
        Value = value;
    }

    public static Result<YearsOfExperience, Error> Create(int value)
    {
        if (value <= 0)
            return Errors.General.ValueCannotBeNegative("Years of experience", value);
        
        return new YearsOfExperience(value);
    }
}