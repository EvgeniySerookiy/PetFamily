namespace PetFamily.Application.Volunteers.PetDTOs;

public record FileDto(
    Stream Content,
    string FileName,
    string ContentType);