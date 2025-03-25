using PetFamily.Domain.PetManagement.PetVO;

namespace PetFamily.Application.FileProvider;

public record PhotoData(
    Stream Stream,
    PhotoPath PhotoPath,
    string BucketName);