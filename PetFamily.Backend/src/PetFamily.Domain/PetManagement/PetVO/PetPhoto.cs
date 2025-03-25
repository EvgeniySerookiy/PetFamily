using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared.ErrorContext;

namespace PetFamily.Domain.PetManagement.PetVO;

public record PetPhoto
{
    public PhotoPath PathToStorage { get; }

    private PetPhoto(PhotoPath pathToStorage)
    {
        PathToStorage = pathToStorage;
    }

    public static Result<PetPhoto, Error> Create(PhotoPath photoPath)
    {
        return new PetPhoto(photoPath);
    }
}