using PetFamily.Application.Volunteers.VolunteerDTOs;

namespace PetFamily.Application.Volunteers.Actions.Volunteers.Update.UpdateRequisitesForHelp;

public record UpdateRequisitesForHelpCommand(
    IEnumerable<RequisitesForHelpDto> RequisitesForHelps);