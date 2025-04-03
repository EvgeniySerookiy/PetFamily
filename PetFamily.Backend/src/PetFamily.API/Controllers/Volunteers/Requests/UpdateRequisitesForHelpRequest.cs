using PetFamily.Application.Volunteers.VolunteerDTOs;

namespace PetFamily.API.Controllers.Requests;

public record UpdateRequisitesForHelpRequest(
    IEnumerable<RequisitesForHelpDto> RequisitesForHelps);