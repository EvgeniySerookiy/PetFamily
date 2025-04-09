using PetFamily.Application.Abstractions;
using PetFamily.Application.Dtos.VolunteerDTOs;

namespace PetFamily.Application.PetManagement.Commands.Volunteers.Update.UpdateRequisitesForHelp;

public record UpdateRequisitesForHelpCommand(
    Guid Id,
    IEnumerable<RequisitesForHelpDto> RequisitesForHelps) : ICommand;