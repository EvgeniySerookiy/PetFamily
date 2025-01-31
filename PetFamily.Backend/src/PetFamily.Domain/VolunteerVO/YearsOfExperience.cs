using CSharpFunctionalExtensions;

namespace PetFamily.Domain.VolunteerVO;

public record YearsOfExperience
{
    public int Value { get; }

    private YearsOfExperience(int value)
    {
        Value = value;
    }

    public static Result<YearsOfExperience> Create(int value)
    {
        if (value < 0)
        {
            return Result.Failure<YearsOfExperience>("Years of experience cannot be negative.");
        }

        var yearsOfExperience = new YearsOfExperience(value);
        
        return Result.Success(yearsOfExperience);
    }
}