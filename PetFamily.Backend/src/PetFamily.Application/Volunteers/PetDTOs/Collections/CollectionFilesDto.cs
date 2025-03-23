using PetFamily.Application.Volunteers.PetDTOs;

namespace PetFamily.Application.Volunteers.DTOs.Collections;

public record CollectionFilesDto(
    IEnumerable<CreateFileDto> Files);