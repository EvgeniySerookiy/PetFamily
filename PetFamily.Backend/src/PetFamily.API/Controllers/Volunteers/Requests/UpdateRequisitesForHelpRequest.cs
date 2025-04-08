using PetFamily.Application.Dtos.VolunteerDTOs;

namespace PetFamily.API.Controllers.Volunteers.Requests;

public record UpdateRequisitesForHelpRequest(
    IEnumerable<RequisitesForHelpDto> RequisitesForHelps);