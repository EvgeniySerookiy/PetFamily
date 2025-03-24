namespace PetFamily.Application.Volunteers.PetDTOs.Collections;

public record CollectionFilesDto(
    IEnumerable<CreateFileDto> Files);