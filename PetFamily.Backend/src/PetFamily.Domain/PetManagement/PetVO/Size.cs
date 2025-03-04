using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared.ErrorContext;

namespace PetFamily.Domain.PetManagement.PetVO;

public record Size
{
    public int Weight { get; }
    public int Height { get; }

    private Size(int weight, int height)
    {
        Weight = weight;
        Height = height;
    }

    public static Result<Size, Error> Create(int weight, int height)
    {
        if (weight <= 0) 
            return Errors.General.ValueCannotBeNegative("Weight", weight);
        
        if (height <= 0) 
            return Errors.General.ValueCannotBeNegative("Height", height);
        
        return new Size(weight, height);
    }
}