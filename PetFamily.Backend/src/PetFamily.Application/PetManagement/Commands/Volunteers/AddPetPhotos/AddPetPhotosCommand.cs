using PetFamily.Application.Abstractions;
using PetFamily.Application.Dtos.PetDTOs;

namespace PetFamily.Application.PetManagement.Commands.Volunteers.AddPetPhotos;

public record AddPetPhotosCommand(
    Guid VolunteerId,
    Guid PetId,
    IEnumerable<CreatePhotoDto> Photos) : ICommand;