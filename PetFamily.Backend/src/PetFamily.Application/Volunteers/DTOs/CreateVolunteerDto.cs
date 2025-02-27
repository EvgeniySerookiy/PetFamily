using PetFamily.Application.Volunteers.Commands;

namespace PetFamily.Application.Volunteers.DTOs;

public record CreateVolunteerDto(
    string FirstName,
    string LastName,
    string MiddleName,
    string Email,
    string Description,
    int YearsOfExperience,
    string PhoneNumber,
    IEnumerable<SocialNetworkDto> SocialNetworks,
    IEnumerable<RequisitesForHelpDto> RequisitesForHelps)
{
    //public CreateVolunteerCommand ToCommand()
    //{
    //    return new CreateVolunteerCommand(
    //        this,
    //        new List<SocialNetworkDto>(),
    //        new List<RequisitesForHelpDto>());
    //}
}