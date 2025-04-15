using PetFamily.Application.Dtos.VolunteerDTOs;
using PetFamily.Application.PetManagement.Commands.Volunteers.CreateVolunteer;
using PetFamily.Application.PetManagement.Commands.Volunteers.UpdateVolunteer.UpdateRequisitesForHelp;
using PetFamily.Application.PetManagement.Commands.Volunteers.UpdateVolunteer.UpdateSocialNetwork;

namespace PetFamily.API.Controllers.Volunteers.Write.Requests;

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