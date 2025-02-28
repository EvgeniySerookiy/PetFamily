using PetFamily.Application.Volunteers.DTOs;

namespace PetFamily.Application.Volunteers.Commands;

public record CreateVolunteerCommand(
    CreateVolunteerDto CreateVolunteerDto,
    IEnumerable<SocialNetworkDto> SocialNetworkLists,
    IEnumerable<RequisitesForHelpDto> RequisitesForHelpLists);