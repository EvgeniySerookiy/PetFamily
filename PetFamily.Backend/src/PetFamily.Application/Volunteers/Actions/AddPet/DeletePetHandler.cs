using CSharpFunctionalExtensions;
using PetFamily.Application.Providers;
using PetFamily.Domain.Shared.ErrorContext;
using PetFamily.Infrastructure.Models;

namespace PetFamily.Application.Volunteers.Actions.AddPet;

public class DeletePetHandler
{
    private readonly IFileProvider _fileProvider;

    public DeletePetHandler(IFileProvider fileProvider)
    {
        _fileProvider = fileProvider;
    }

    public async Task<Result<string, Error>> Handle(
        Guid fieldId,
        CancellationToken cancellationToken = default)
    {
        return await _fileProvider.DeleteFile(fieldId, cancellationToken);
    }
}