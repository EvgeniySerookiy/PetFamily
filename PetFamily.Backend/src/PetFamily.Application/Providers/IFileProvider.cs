using CSharpFunctionalExtensions;
using PetFamily.Application.Photos;
using PetFamily.Domain.PetManagement.PetVO;
using PetFamily.Domain.Shared.ErrorContext;

namespace PetFamily.Application.Providers;

public interface IFileProvider
{
    Task<Result<IReadOnlyList<PhotoPath>, Error>> UploadPhotos(
        IEnumerable<PhotoData> photosData,
        CancellationToken cancellationToken = default);
    
    Task<UnitResult<Error>> DeletePhotos(
        PhotosPathWithBucket pathWithBucket,
        CancellationToken cancellationToken = default);

    Task<UnitResult<Error>> DeletePhoto(
        PhotoInfo photoInfo,
        CancellationToken cancellationToken = default);
}