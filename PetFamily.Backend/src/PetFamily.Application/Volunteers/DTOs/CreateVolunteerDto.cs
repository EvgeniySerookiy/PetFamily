using PetFamily.Domain.PetManagement.VolunteerVO;

namespace PetFamily.Application.Volunteers.DTOs;

public record CreateVolunteerDto(
    FullNameDto FullName,
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