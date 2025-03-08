using PetFamily.Application.Volunteers.DTOs.Collections;

namespace PetFamily.Application.Volunteers.Requests;

public record UpdateCollectionSocialNetworkRequest(
    Guid VolunteerId,
    CollectionSocialNetworkDto CollectionSocialNetwork);