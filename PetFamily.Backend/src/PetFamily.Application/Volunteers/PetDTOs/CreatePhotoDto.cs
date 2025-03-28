namespace PetFamily.Application.Volunteers.PetDTOs;

public record CreatePhotoDto(
    Stream Content,
    string PhotoName,
    string ContentType);