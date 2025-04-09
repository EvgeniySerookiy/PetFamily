namespace PetFamily.Application.Dtos.PetDTOs;

public record CreatePhotoDto(
    Stream Content,
    string PhotoName,
    string ContentType);