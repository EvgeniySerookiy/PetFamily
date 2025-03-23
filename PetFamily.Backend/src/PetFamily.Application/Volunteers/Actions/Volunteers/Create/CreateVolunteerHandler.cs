using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Domain.PetManagement.AggregateRoot;
using PetFamily.Domain.PetManagement.SharedVO;
using PetFamily.Domain.PetManagement.VolunteerVO;
using PetFamily.Domain.Shared.ErrorContext;
using RequisitesForHelp = PetFamily.Domain.PetManagement.VolunteerVO.RequisitesForHelp;
using SocialNetwork = PetFamily.Domain.PetManagement.VolunteerVO.SocialNetwork;

namespace PetFamily.Application.Volunteers.Actions.Create;

public class CreateVolunteerHandler
{
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly ILogger<CreateVolunteerHandler> _logger;

    public CreateVolunteerHandler(
        IVolunteersRepository volunteersRepository,
        ILogger<CreateVolunteerHandler> logger)
    {
        _volunteersRepository = volunteersRepository;
        _logger = logger;
    }

    public async Task<Result<Guid, Error>> Handle(
        CreateVolunteerRequest request,
        CancellationToken cancellationToken = default)
    {
        var volunteerId = VolunteerId.NewVolunteerId();

        var fullName = FullName.Create(
            request.MainInfo.FullName.FirstName,
            request.MainInfo.FullName.LastName,
            request.MainInfo.FullName.MiddleName).Value;
        
        var email = Email.Create(request.MainInfo.Email).Value;
        var description = Description.Create(request.MainInfo.Description).Value;
        var yearsOfExperience = YearsOfExperience.Create(request.MainInfo.YearsOfExperience).Value;
        var phoneNumber = PhoneNumber.Create(request.MainInfo.PhoneNumber).Value;
        
        var socialNetworks = request.CollectionSocialNetwork.SocialNetworks;
        var socialNetworkList = new List<SocialNetwork>();
        foreach (var socialNetwork in socialNetworks)
        {
            var value = SocialNetwork.Create(
                socialNetwork.NetworkName, 
                socialNetwork.NetworkAddress).Value;
            
            socialNetworkList.Add(value);
        }
        
        var requisitesForHelps = request.CollectionRequisitesForHelp.RequisitesForHelps;
        var requisitesForHelpList = new List<RequisitesForHelp>();
        foreach (var requisitesForHelp in requisitesForHelps)
        {
            var value = RequisitesForHelp.Create(
                requisitesForHelp.Recipient,
                requisitesForHelp.PaymentDetails).Value;
            
            requisitesForHelpList.Add(value);
        }
        
        var volunteer = await _volunteersRepository.GetByEmail(email, cancellationToken);
        
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
        
        _logger.LogInformation("Created volunteer {volunteer} with id {volunteerId}", volunteer, volunteerId);
        
        return volunteerId.Value;
    }
}