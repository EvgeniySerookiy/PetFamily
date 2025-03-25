using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared.ErrorContext;

namespace PetFamily.Domain.PetManagement.PetVO;

public record Size
{
    public double Weight { get; }
    public double Height { get; }

    private Size(double weight, double height)
    {
        Weight = weight;
        Height = height;
    }

    public static Result<Size, Error> Create(double weight, double height)
    {
        if (weight <= 0) 
            return Errors.General.ValueCannotBeNegative("Weight", weight);
        
        if (height <= 0) 
            return Errors.General.ValueCannotBeNegative("Height", height);
        
        return new Size(weight, height);
    }
}