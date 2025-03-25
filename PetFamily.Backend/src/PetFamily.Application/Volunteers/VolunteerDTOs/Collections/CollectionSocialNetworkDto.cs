namespace PetFamily.Application.Volunteers.VolunteerDTOs.Collections;

public record CollectionSocialNetworkDto(
    IEnumerable<SocialNetworkDto> SocialNetworks);