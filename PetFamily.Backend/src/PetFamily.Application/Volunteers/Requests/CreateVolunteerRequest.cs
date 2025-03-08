using PetFamily.Application.Volunteers.DTOs;
using PetFamily.Application.Volunteers.DTOs.Collections;

namespace PetFamily.Application.Volunteers.Requests;

public record CreateVolunteerRequest(
    MainInfoDto MainInfo,
    CollectionSocialNetworkDto CollectionSocialNetwork,
    CollectionRequisitesForHelpDto CollectionRequisitesForHelp);