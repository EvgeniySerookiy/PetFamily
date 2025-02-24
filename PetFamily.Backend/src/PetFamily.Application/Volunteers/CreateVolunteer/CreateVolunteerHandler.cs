using CSharpFunctionalExtensions;
using PetFamily.Application.Volunteers.Commands;
using PetFamily.Domain.Shared.ErrorContext;
using PetFamily.Domain.SharedVO;
using PetFamily.Domain.VolunteerContext.VolunteerVO;
using RequisitesForHelp = PetFamily.Domain.VolunteerContext.VolunteerVO.RequisitesForHelp;
using SocialNetwork = PetFamily.Domain.VolunteerContext.VolunteerVO.SocialNetwork;

namespace PetFamily.Application.Volunteers.CreateVolunteer;

public class CreateVolunteerHandler
{
    private readonly IVolunteersRepository _volunteersRepository;

    public CreateVolunteerHandler(IVolunteersRepository volunteersRepository)
    {
        _volunteersRepository = volunteersRepository;
    }
    
    public async Task<Result<Guid, Error>> Handle(
        CreateVolunteerCommand command,
        CancellationToken cancellationToken = default)
    {
        var volunteerId = VolunteerId.NewVolunteerId();
        //var fullNameResult = FullName.Create("Evgeniy", "Serookiy", "Sergeevich");
        var fullNameResult = FullName.Create(command.CreateVolunteerRequest.FirstName, 
            command.CreateVolunteerRequest.LastName, 
            command.CreateVolunteerRequest.MiddleName);

        var emailResult = Email.Create("EvgeniySerк@gmail.com");
        var descriptionResult = Description.Create(
            "The asymptote, without going into details, is negative. " +
            "The minimum, as follows from the above, is irrational. The " +
            "function of many variables, excluding the obvious case, transforms " +
            "the empirical determinant, which undoubtedly leads us to the truth. " +
            "Higher arithmetic, as follows from the above, is still in demand.");
        var yearsOfExperienceResult = YearsOfExperience.Create(5);
        var phoneNumberResult = PhoneNumber.Create("+375 (67) 89 87 340");

        var requisitesForHelpBankTht = RequisitesForHelp.Create("SerookiyEvgeniy",
            "3456789034567890");
        var requisitesForHelpBankGre = RequisitesForHelp.Create("SerookiyEvgeniy",
            "3456789045678678");
        var transferRequisitesForHelpsList = TransferRequisitesForHelpsList.Create(
            new[]
            {
                requisitesForHelpBankTht.Value,
                requisitesForHelpBankGre.Value
            });

        var socialNetworkFacebookResult = SocialNetwork.Create("Facebook",
            "https://www.facebook.com/profile.php?id=100062023033067&locale=ru_RU");
        var socialNetworkTelegramResult = SocialNetwork.Create("Telegram", "@dfghjkl;");
        var transferSocialNetworkList = TransferSocialNetworkList.Create(
            new[]
        {
            socialNetworkFacebookResult.Value, 
            socialNetworkTelegramResult.Value
        });
        
        var existVolunteer = await _volunteersRepository.GetByEmail(emailResult.Value);

        if (existVolunteer.IsSuccess)
            return Errors.Volunteer.AlreadyExist();
        
        var volunteerToCreate = Domain.VolunteerContext.Volunteer.Create(
            volunteerId, 
            fullNameResult.Value,
            emailResult.Value,
            descriptionResult.Value,
            yearsOfExperienceResult.Value,
            phoneNumberResult.Value,
            transferRequisitesForHelpsList.Value,
            transferSocialNetworkList.Value);
        
        await _volunteersRepository.Add(volunteerToCreate.Value, cancellationToken);
        
        return volunteerId.Value;
    }
}