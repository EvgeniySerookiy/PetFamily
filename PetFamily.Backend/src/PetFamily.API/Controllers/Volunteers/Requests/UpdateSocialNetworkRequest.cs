using PetFamily.Application.Volunteers.VolunteerDTOs;

namespace PetFamily.API.Controllers.Requests;

public record UpdateSocialNetworkRequest(
    IEnumerable<SocialNetworkDto> SocialNetworks);