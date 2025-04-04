using PetFamily.Application.Dtos.VolunteerDTOs;

namespace PetFamily.API.Controllers.Requests;

public record UpdateSocialNetworkRequest(
    IEnumerable<SocialNetworkDto> SocialNetworks);