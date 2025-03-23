using CSharpFunctionalExtensions;
using PetFamily.Application.FileProvider;
using PetFamily.Application.Volunteers.Actions.Pets.AddPet;
using PetFamily.Domain.Shared.ErrorContext;

namespace PetFamily.Application.Providers;

public interface IFileProvider
{
    Task<UnitResult<Error>> UploadFiles(
        FileData fileData,
        CancellationToken cancellationToken = default);
    
    Task<UnitResult<Error>> DeleteFiles(
        CollectionsObjectName collectionsObjectName,
        CancellationToken cancellationToken = default);
    
    Task<Result<string, Error>> GetFileDownloadUrl(
        Guid fieldId);
}