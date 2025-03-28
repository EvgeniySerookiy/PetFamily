using PetFamily.Application.Volunteers.Actions.Volunteers.Update.UpdateRequisitesForHelp;
using PetFamily.Application.Volunteers.Actions.Volunteers.Update.UpdateSocialNetwork;
using PetFamily.Application.Volunteers.VolunteerDTOs;

namespace PetFamily.Application.Volunteers.Actions.Volunteers.Create;

public record CreateVolunteerCommand(
    MainInfoDto MainInfo,
    UpdateSocialNetworksCommand UpdateSocialNetwork,
    UpdateRequisitesForHelpCommand UpdateRequisitesForHelp);