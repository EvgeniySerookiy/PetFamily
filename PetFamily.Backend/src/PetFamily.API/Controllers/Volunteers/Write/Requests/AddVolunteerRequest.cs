using PetFamily.Application.Dtos.VolunteerDTOs;
using PetFamily.Application.PetManagement.Commands.Volunteers.AddVolunteer;
using PetFamily.Application.PetManagement.Commands.Volunteers.UpdateVolunteer.UpdateRequisitesForHelp;
using PetFamily.Application.PetManagement.Commands.Volunteers.UpdateVolunteer.UpdateSocialNetwork;

namespace PetFamily.API.Controllers.Volunteers.Write.Requests;

public record AddVolunteerRequest(
    MainInfoDto MainInfo,
    UpdateSocialNetworksCommand UpdateSocialNetwork,
    UpdateRequisitesForHelpCommand UpdateRequisitesForHelp)
{
    public AddVolunteerCommand ToCommand() => 
        new (
        MainInfo, 
        UpdateSocialNetwork, 
        UpdateRequisitesForHelp);
}