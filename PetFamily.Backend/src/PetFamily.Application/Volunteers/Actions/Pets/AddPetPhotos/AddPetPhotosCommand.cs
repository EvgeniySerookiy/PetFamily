using PetFamily.Application.Volunteers.PetDTOs;

namespace PetFamily.Application.Volunteers.Actions.Pets.AddPetPhotos;

public record AddPetPhotosCommand(
    IEnumerable<CreatePhotoDto> Photos);