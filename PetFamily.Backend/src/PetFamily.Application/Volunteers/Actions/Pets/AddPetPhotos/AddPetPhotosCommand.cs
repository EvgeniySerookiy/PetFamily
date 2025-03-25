using PetFamily.Application.Volunteers.PetDTOs.Collections;

namespace PetFamily.Application.Volunteers.Actions.Pets.AddPetPhotos;

public record AddPetPhotosCommand(
    Guid VolunteerId,
    Guid PetId,
    CollectionFilesDto CollectionFiles);