using PetFamily.Application.Abstractions;

namespace PetFamily.Application.PetManagement.Commands.Volunteers.DeletePet;

public record DeletePetCommand(
    Guid VolunteerId,
    Guid PetId) : ICommand;
