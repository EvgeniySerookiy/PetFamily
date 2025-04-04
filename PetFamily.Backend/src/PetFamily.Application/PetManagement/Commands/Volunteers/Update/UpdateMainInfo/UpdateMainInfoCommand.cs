using PetFamily.Application.Abstractions;
using PetFamily.Application.Dtos.VolunteerDTOs;

namespace PetFamily.Application.PetManagement.Commands.Volunteers.Update.UpdateMainInfo;

public record UpdateMainInfoCommand(
    Guid VolunteerId,
    MainInfoDto MainInfo) : ICommand;