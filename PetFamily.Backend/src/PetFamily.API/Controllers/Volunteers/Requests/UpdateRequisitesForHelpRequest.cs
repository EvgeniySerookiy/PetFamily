using PetFamily.Application.Dtos.VolunteerDTOs;

namespace PetFamily.API.Controllers.Requests;

public record UpdateRequisitesForHelpRequest(
    IEnumerable<RequisitesForHelpDto> RequisitesForHelps);