using PetFamily.Application.Volunteers.DTOs.Collections;
using PetFamily.Application.Volunteers.PetDTOs;

namespace PetFamily.Application.Volunteers.Actions.Pets.AddPet;

public record AddPetCommand(
    Guid VolunteerId,
    MainPetInfoDto MainPetInfo,
    CollectionFilesDto CollectionFiles);