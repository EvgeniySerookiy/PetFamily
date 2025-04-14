using PetFamily.Application.Abstractions;
using PetFamily.Application.Dtos.VolunteerDTOs;

namespace PetFamily.Application.PetManagement.Commands.Volunteers.UpdateVolunteer.UpdateRequisitesForHelp;

public record UpdateRequisitesForHelpCommand(
    Guid Id,
    IEnumerable<RequisitesForHelpDto> RequisitesForHelps) : ICommand;