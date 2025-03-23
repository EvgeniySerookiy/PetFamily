using PetFamily.Application.Volunteers.DTOs;
using PetFamily.Application.Volunteers.DTOs.Collections;

namespace PetFamily.Application.Volunteers.Actions.Create;

public record CreateVolunteerRequest(
    MainInfoDto MainInfo,
    CollectionSocialNetworkDto CollectionSocialNetwork,
    CollectionRequisitesForHelpDto CollectionRequisitesForHelp);