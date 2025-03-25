using PetFamily.Application.Volunteers.VolunteerDTOs.Collections;

namespace PetFamily.Application.Volunteers.Actions.Volunteers.Update.UpdateSocialNetwork;

public record UpdateCollectionSocialNetworkRequest(
    Guid VolunteerId,
    CollectionSocialNetworkDto CollectionSocialNetwork);