using PetFamily.Application.Volunteers.VolunteerDTOs;

namespace PetFamily.Application.Volunteers.Actions.Volunteers.Update.UpdateSocialNetwork;

public record UpdateSocialNetworksCommand(
    IEnumerable<SocialNetworkDto> SocialNetworks);