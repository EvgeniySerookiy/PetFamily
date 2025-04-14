using PetFamily.Application.Abstractions;
using PetFamily.Application.Dtos.VolunteerDTOs;

namespace PetFamily.Application.PetManagement.Commands.Volunteers.UpdateVolunteer.UpdateMainInfo;

public record UpdateMainInfoCommand(
    Guid VolunteerId,
    MainInfoDto MainInfo) : ICommand;