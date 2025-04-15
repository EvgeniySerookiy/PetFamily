using PetFamily.Application.Abstractions;

namespace PetFamily.Application.PetManagement.Commands.Volunteers.DeleteVolunteer;

public record DeleteVolunteerCommand(
    Guid Id) : ICommand;