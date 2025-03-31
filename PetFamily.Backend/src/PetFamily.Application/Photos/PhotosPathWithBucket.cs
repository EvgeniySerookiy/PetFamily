using PetFamily.Domain.PetManagement.PetVO;

namespace PetFamily.Application.Photos;

public record PhotosPathWithBucket(
    IEnumerable<PhotoPath> PhotosPath,
    string BucketName);