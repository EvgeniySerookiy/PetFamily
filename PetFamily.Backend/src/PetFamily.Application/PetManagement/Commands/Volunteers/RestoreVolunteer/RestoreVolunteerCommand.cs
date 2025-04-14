using PetFamily.Application.Abstractions;

namespace PetFamily.Application.PetManagement.Commands.Volunteers.RestoreVolunteer;

public record RestoreVolunteerCommand(
    Guid Id) : ICommand;