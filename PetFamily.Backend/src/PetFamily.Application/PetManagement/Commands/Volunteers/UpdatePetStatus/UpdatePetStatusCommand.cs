using PetFamily.Application.Abstractions;
using PetFamily.Domain;

namespace PetFamily.Application.PetManagement.Commands.Volunteers.UpdatePetStatus;

public record UpdatePetStatusCommand(
    Guid VolunteerId,
    Guid PetId,
    AssistanceStatus Status) : ICommand;