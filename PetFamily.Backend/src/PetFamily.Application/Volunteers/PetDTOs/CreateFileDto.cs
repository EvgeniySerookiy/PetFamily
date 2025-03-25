namespace PetFamily.Application.Volunteers.PetDTOs;

public record CreateFileDto(
    Stream Content,
    string FileName,
    string ContentType);