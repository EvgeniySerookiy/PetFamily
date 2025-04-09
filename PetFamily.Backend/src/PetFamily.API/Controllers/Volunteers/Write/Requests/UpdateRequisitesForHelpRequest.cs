using PetFamily.Application.Dtos.VolunteerDTOs;

namespace PetFamily.API.Controllers.Volunteers.Write.Requests;

public record UpdateRequisitesForHelpRequest(
    IEnumerable<RequisitesForHelpDto> RequisitesForHelps);