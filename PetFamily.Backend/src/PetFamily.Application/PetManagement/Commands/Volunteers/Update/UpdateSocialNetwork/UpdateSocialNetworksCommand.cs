using PetFamily.Application.Abstractions;
using PetFamily.Application.Dtos.VolunteerDTOs;

namespace PetFamily.Application.PetManagement.Commands.Volunteers.Update.UpdateSocialNetwork;

public record UpdateSocialNetworksCommand(
    Guid Id,
    IEnumerable<SocialNetworkDto> SocialNetworks) : ICommand;