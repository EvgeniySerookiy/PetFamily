using PetFamily.Application.Abstractions;

namespace PetFamily.Application.PetManagement.Commands.Pets.DeletePetPhotos;

public record DeletePetPhotosCommand(
    Guid VolunteerId,
    Guid PetId,
    IEnumerable<Guid> PhotosId) : ICommand;