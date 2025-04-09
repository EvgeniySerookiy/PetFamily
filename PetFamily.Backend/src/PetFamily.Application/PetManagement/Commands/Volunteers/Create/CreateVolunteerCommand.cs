using PetFamily.Application.Abstractions;
using PetFamily.Application.Dtos.VolunteerDTOs;
using PetFamily.Application.PetManagement.Commands.Volunteers.Update.UpdateRequisitesForHelp;
using PetFamily.Application.PetManagement.Commands.Volunteers.Update.UpdateSocialNetwork;

namespace PetFamily.Application.PetManagement.Commands.Volunteers.Create;

public record CreateVolunteerCommand(
    MainInfoDto MainInfo,
    UpdateSocialNetworksCommand UpdateSocialNetwork,
    UpdateRequisitesForHelpCommand UpdateRequisitesForHelp) : ICommand;