using PetFamily.Domain.Shared;

namespace PetFamily.Domain.PetContext.PetVO;

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
            return "Weight must be greater than zero.";
        
        if (height <= 0) 
            return "Height must be greater than zero.";
        
        var petSize = new Size(weight, height);
        
        return petSize;
    }
}