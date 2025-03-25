using PetFamily.Domain.PetManagement.PetVO;

namespace PetFamily.Application.FileProvider;

public record PhotosPathWithBucket(
    IEnumerable<PhotoPath> PhotosPath,
    string BucketName);