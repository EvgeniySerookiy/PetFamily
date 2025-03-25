using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared.ErrorContext;

namespace PetFamily.Domain.PetManagement.PetVO;

public record PhotoPath
{
    public string Path { get; }

    private PhotoPath(string path)
    {
        Path = path;
    }

    public static Result<PhotoPath, Error> Create(Guid path, string extension)
    {
        // Валидация на доступные расширения файлов
        var fullPath = path + "." + extension;
        
        return new PhotoPath(fullPath);
    }
    
    public static Result<PhotoPath, Error> Create(string fullPath)
    {
        return new PhotoPath(fullPath);
    }
}