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

    public CreateVolunteerHandler(
        IVolunteersRepository volunteersRepository)
    {
        _volunteersRepository = volunteersRepository;
    }

    public async Task<Result<Guid, Error>> Handle(
        CreateVolunteerDto request,
        CancellationToken cancellationToken = default)
    {
        var volunteerId = VolunteerId.NewVolunteerId();

        var fullName = FullName.Create(
            request.FirstName,
            request.LastName,
            request.MiddleName).Value;
        
        var email = Email.Create(request.Email).Value;
        var description = Description.Create(request.Description).Value;
        var yearsOfExperience = YearsOfExperience.Create(request.YearsOfExperience).Value;
        var phoneNumber = PhoneNumber.Create(request.PhoneNumber).Value;
        
        var socialNetworks = request.SocialNetworks;
        var socialNetworkList = new List<SocialNetwork>();
        foreach (var socialNetwork in socialNetworks)
        {
            var value = SocialNetwork.Create(
                socialNetwork.NetworkName, 
                socialNetwork.NetworkAddress).Value;
            
            socialNetworkList.Add(value);
        }
        
        var requisitesForHelps = request.RequisitesForHelps;
        var requisitesForHelpList = new List<RequisitesForHelp>();
        foreach (var requisitesForHelp in requisitesForHelps)
        {
            var value = RequisitesForHelp.Create(
                requisitesForHelp.Recipient,
                requisitesForHelp.PaymentDetails).Value;
            
            requisitesForHelpList.Add(value);
        }
        
        var volunteer = await _volunteersRepository.GetByEmail(email);
        
        if (volunteer.IsSuccess)
            return Errors.Volunteer.AlreadyExist();
        
        var volunteerToCreate = Volunteer.Create(
            volunteerId, 
            fullName,
            email,
            description,
            yearsOfExperience,
            phoneNumber,
            TransferRequisitesForHelpsList.Create(requisitesForHelpList).Value,
            TransferSocialNetworkList.Create(socialNetworkList).Value);
        
        await _volunteersRepository.Add(volunteerToCreate.Value, cancellationToken);
        
        return volunteerId.Value;
    }
}