using PetFamily.Application.Abstractions;
using PetFamily.Application.Dtos.VolunteerDTOs;
using PetFamily.Application.PetManagement.Commands.Volunteers.UpdateVolunteer.UpdateRequisitesForHelp;
using PetFamily.Application.PetManagement.Commands.Volunteers.UpdateVolunteer.UpdateSocialNetwork;

namespace PetFamily.Application.PetManagement.Commands.Volunteers.AddVolunteer;

public record AddVolunteerCommand(
    MainInfoDto MainInfo,
    UpdateSocialNetworksCommand UpdateSocialNetwork,
    UpdateRequisitesForHelpCommand UpdateRequisitesForHelp) : ICommand;