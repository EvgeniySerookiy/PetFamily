using PetFamily.Application.Volunteers.Actions.Volunteers.Create;
using PetFamily.Application.Volunteers.Actions.Volunteers.Update.UpdateRequisitesForHelp;
using PetFamily.Application.Volunteers.Actions.Volunteers.Update.UpdateSocialNetwork;
using PetFamily.Application.Volunteers.VolunteerDTOs;

namespace PetFamily.API.Controllers.Requests;

public record CreateVolunteerRequest(
    MainInfoDto MainInfo,
    UpdateSocialNetworksCommand UpdateSocialNetwork,
    UpdateRequisitesForHelpCommand UpdateRequisitesForHelp)
{
    public CreateVolunteerCommand ToCommand() => 
        new (
        MainInfo, 
        UpdateSocialNetwork, 
        UpdateRequisitesForHelp);
}