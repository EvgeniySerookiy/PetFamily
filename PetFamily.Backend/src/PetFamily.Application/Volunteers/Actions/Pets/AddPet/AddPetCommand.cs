using PetFamily.Application.Volunteers.PetDTOs;
using PetFamily.Application.Volunteers.PetDTOs.Collections;

namespace PetFamily.Application.Volunteers.Actions.Pets.AddPet;

public record AddPetCommand(
    Guid VolunteerId,
    MainPetInfoDto MainPetInfo,
    CollectionFilesDto CollectionFiles);