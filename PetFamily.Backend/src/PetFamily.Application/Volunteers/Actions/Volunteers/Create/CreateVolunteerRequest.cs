using PetFamily.Application.Volunteers.VolunteerDTOs;
using PetFamily.Application.Volunteers.VolunteerDTOs.Collections;

namespace PetFamily.Application.Volunteers.Actions.Volunteers.Create;

public record CreateVolunteerRequest(
    MainInfoDto MainInfo,
    CollectionSocialNetworkDto CollectionSocialNetwork,
    CollectionRequisitesForHelpDto CollectionRequisitesForHelp);