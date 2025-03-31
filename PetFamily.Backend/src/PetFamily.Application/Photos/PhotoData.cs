using PetFamily.Domain.PetManagement.PetVO;

namespace PetFamily.Application.Photos;

public record PhotoData(
    Stream Stream,
    PhotoInfo PhotoInfo);

public record PhotoInfo(
    PhotoPath PhotoPath,
    string BucketName);
