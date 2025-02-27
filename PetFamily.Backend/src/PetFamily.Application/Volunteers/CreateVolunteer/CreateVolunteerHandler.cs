using CSharpFunctionalExtensions;
using PetFamily.Application.Volunteers.DTOs;
using PetFamily.Domain.PetManagement.AggregateRoot;
using PetFamily.Domain.PetManagement.SharedVO;
using PetFamily.Domain.PetManagement.VolunteerVO;
using PetFamily.Domain.Shared.ErrorContext;
using RequisitesForHelp = PetFamily.Domain.PetManagement.VolunteerVO.RequisitesForHelp;
using SocialNetwork = PetFamily.Domain.PetManagement.VolunteerVO.SocialNetwork;

namespace PetFamily.Application.Volunteers.CreateVolunteer;

public class CreateVolunteerHandler
{
    private readonly IVolunteersRepository _volunteersRepository;

    public CreateVolunteerHandler(IVolunteersRepository volunteersRepository)
    {
        _volunteersRepository = volunteersRepository;
    }

    public async Task<Result<Guid, Error>> Handle(
        CreateVolunteerDto request,
        CancellationToken cancellationToken = default)
    {
        var volunteerId = VolunteerId.NewVolunteerId();

        var fullNameResult = FullName.Create(
            request.FirstName,
            request.LastName,
            request.MiddleName);
        if (fullNameResult.IsFailure)
            return fullNameResult.Error;

        var emailResult = Email.Create(request.Email);
        if (emailResult.IsFailure)
            return emailResult.Error;

        var descriptionResult = Description.Create(request.Description);
        if (descriptionResult.IsFailure)
            return descriptionResult.Error;

        var yearsOfExperienceResult = YearsOfExperience.Create(request.YearsOfExperience);
        if (yearsOfExperienceResult.IsFailure)
            return yearsOfExperienceResult.Error;

        var phoneNumberResult = PhoneNumber.Create(request.PhoneNumber);
        if (phoneNumberResult.IsFailure)
            return phoneNumberResult.Error;

        var socialNetworks = request.SocialNetworks;
        var socialNetworkList = new List<SocialNetwork>();
        foreach (var socialNetwork in socialNetworks)
        {
            var socialNetworkResult = SocialNetwork.Create(
                socialNetwork.NetworkName, 
                socialNetwork.NetworkAddress);
            
            if(socialNetworkResult.IsFailure)
                return socialNetworkResult.Error;
            
            socialNetworkList.Add(socialNetworkResult.Value);
        }
        
        var requisitesForHelps = request.RequisitesForHelps;
        var requisitesForHelpList = new List<RequisitesForHelp>();
        foreach (var requisitesForHelp in requisitesForHelps)
        {
            var requisitesForHelpResult = RequisitesForHelp.Create(
                requisitesForHelp.Recipient,
                requisitesForHelp.PaymentDetails);
            
            if(requisitesForHelpResult.IsFailure)
                return requisitesForHelpResult.Error;
            
            requisitesForHelpList.Add(requisitesForHelpResult.Value);
        }
        
        var volunteerToCreate = Volunteer.Create(
            volunteerId, 
            fullNameResult.Value,
            emailResult.Value,
            descriptionResult.Value,
            yearsOfExperienceResult.Value,
            phoneNumberResult.Value,
            TransferRequisitesForHelpsList.Create(requisitesForHelpList).Value,
            TransferSocialNetworkList.Create(socialNetworkList).Value);
        
        await _volunteersRepository.Add(volunteerToCreate.Value, cancellationToken);
        
        return volunteerId.Value;
    }
}