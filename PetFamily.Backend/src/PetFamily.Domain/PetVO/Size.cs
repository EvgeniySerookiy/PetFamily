using CSharpFunctionalExtensions;

namespace PetFamily.Domain.PetVO;

public record Size
{
    public int Weight { get; }
    public int Height { get; }

    private Size(int weight, int height)
    {
        Weight = weight;
        Height = height;
    }

    public static Result<Size> Create(int weight, int height)
    {
        if (weight <= 0) 
            return Result.Failure<Size>("Weight must be greater than zero.");
        
        if (height <= 0) 
            return Result.Failure<Size>("Height must be greater than zero.");
        
        var petSize = new Size(weight, height);
        
        return Result.Success(petSize);
    }
}