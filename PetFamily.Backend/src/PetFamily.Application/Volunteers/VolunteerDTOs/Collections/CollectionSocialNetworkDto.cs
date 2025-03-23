namespace PetFamily.Application.Volunteers.DTOs.Collections;

public record CollectionSocialNetworkDto(
    IEnumerable<SocialNetworkDto> SocialNetworks);