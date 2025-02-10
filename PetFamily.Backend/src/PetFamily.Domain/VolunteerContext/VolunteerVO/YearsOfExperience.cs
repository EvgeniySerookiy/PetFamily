using PetFamily.Domain.Shared;

namespace PetFamily.Domain.VolunteerContext.VolunteerVO;

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
            return "Years of experience cannot be negative.";
        }

        var yearsOfExperience = new YearsOfExperience(value);
        
        return yearsOfExperience;
    }
}