using PetFamily.Application.Dtos.VolunteerDTOs;
using PetFamily.Application.PetManagement.Commands.Volunteers.Create;
using PetFamily.Application.PetManagement.Commands.Volunteers.Update.UpdateRequisitesForHelp;
using PetFamily.Application.PetManagement.Commands.Volunteers.Update.UpdateSocialNetwork;

namespace PetFamily.API.Controllers.Volunteers.Requests;

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