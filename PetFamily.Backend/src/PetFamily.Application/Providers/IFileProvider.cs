using CSharpFunctionalExtensions;
using PetFamily.Application.FileProvider;
using PetFamily.Domain.PetManagement.PetVO;
using PetFamily.Domain.Shared.ErrorContext;

namespace PetFamily.Application.Providers;

public interface IFileProvider
{
    Task<Result<IReadOnlyList<PhotoPath>, Error>> UploadFiles(
        IEnumerable<PhotoData> photosData,
        CancellationToken cancellationToken = default);
    
    Task<UnitResult<Error>> DeleteFiles(
        PhotosPathWithBucket pathWithBucket,
        CancellationToken cancellationToken = default);
    
    Task<Result<string, Error>> GetFileDownloadUrl(
        Guid fieldId);
}