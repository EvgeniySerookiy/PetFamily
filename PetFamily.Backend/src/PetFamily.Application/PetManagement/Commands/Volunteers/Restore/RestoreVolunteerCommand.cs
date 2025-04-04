using PetFamily.Application.Abstractions;

namespace PetFamily.Application.PetManagement.Commands.Volunteers.Restore;

public record RestoreVolunteerCommand(
    Guid Id) : ICommand;