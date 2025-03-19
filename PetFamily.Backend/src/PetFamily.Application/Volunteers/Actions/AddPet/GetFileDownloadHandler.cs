using CSharpFunctionalExtensions;
using PetFamily.Application.Providers;
using PetFamily.Domain.Shared.ErrorContext;

namespace PetFamily.Application.Volunteers.Actions.AddPet;

public class GetFileDownloadHandler
{
    private readonly IFileProvider _fileProvider;

    public GetFileDownloadHandler(IFileProvider fileProvider)
    {
        _fileProvider = fileProvider;
    }

    public async Task<Result<string, Error>> Handle(
        Guid fieldId)
    {
        return await _fileProvider.GetFileDownloadUrl(fieldId);
    }
}