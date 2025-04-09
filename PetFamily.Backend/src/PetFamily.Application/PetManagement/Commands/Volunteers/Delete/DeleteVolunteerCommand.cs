using PetFamily.Application.Abstractions;

namespace PetFamily.Application.PetManagement.Commands.Volunteers.Delete;

public record DeleteVolunteerCommand(
    Guid Id) : ICommand;