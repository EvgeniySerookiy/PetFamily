using PetFamily.Application.Volunteers.DTOs;

namespace PetFamily.Application.Volunteers.Commands;

public record CreateVolunteerCommand(
    CreateVolunteerRequest CreateVolunteerRequest,
    IEnumerable<SocialNetwork> SocialNetworkList,
    IEnumerable<RequisitesForHelp> RequisitesForHelpList);