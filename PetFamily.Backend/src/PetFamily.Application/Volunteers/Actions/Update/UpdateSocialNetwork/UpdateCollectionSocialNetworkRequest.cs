using PetFamily.Application.Volunteers.DTOs.Collections;

namespace PetFamily.Application.Volunteers.Actions.Update.UpdateSocialNetwork;

public record UpdateCollectionSocialNetworkRequest(
    Guid VolunteerId,
    CollectionSocialNetworkDto CollectionSocialNetwork);