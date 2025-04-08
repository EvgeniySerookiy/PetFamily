using PetFamily.Application.Dtos.VolunteerDTOs;

namespace PetFamily.API.Controllers.Volunteers.Write.Requests;

public record UpdateSocialNetworkRequest(
    IEnumerable<SocialNetworkDto> SocialNetworks);