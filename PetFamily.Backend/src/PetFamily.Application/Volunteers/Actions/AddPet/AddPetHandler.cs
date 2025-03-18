using CSharpFunctionalExtensions;
using PetFamily.Application.Providers;
using PetFamily.Domain.Shared.ErrorContext;
using PetFamily.Infrastructure.Models;

namespace PetFamily.Application.Volunteers.Actions.AddPet;

public class AddPetHandler
{
    private readonly IFileProvider _fileProvider;

    public AddPetHandler(IFileProvider fileProvider)
    {
        _fileProvider = fileProvider;
    }

    public async Task<Result<string, Error>> Handle(
        FileData fileData,
        CancellationToken cancellationToken = default)
    {
        return await _fileProvider.UploadFile(fileData, cancellationToken);
    }
} 