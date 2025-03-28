using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Database;
using PetFamily.Application.Extensions;
using PetFamily.Domain.PetManagement.AggregateRoot;
using PetFamily.Domain.PetManagement.SharedVO;
using PetFamily.Domain.PetManagement.VolunteerVO;
using PetFamily.Domain.Shared.ErrorContext;
using RequisitesForHelp = PetFamily.Domain.PetManagement.VolunteerVO.RequisitesForHelp;
using SocialNetwork = PetFamily.Domain.PetManagement.VolunteerVO.SocialNetwork;

namespace PetFamily.Application.Volunteers.Actions.Volunteers.Create;

public class CreateVolunteerHandler
{
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly ILogger<CreateVolunteerHandler> _logger;
    private readonly IValidator<CreateVolunteerCommand> _validator;

    public CreateVolunteerHandler(
        IVolunteersRepository volunteersRepository,
        ILogger<CreateVolunteerHandler> logger,
        IValidator<CreateVolunteerCommand> validator)
    {
        _volunteersRepository = volunteersRepository;
        _logger = logger;
        _validator = validator;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        CreateVolunteerCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToErrorList();
        
        var volunteerId = VolunteerId.NewVolunteerId();

        var fullName = FullName.Create(
            command.MainInfo.FullName.FirstName,
            command.MainInfo.FullName.LastName,
            command.MainInfo.FullName.MiddleName).Value;
        
        var email = Email.Create(command.MainInfo.Email).Value;
        var description = Description.Create(command.MainInfo.Description).Value;
        var yearsOfExperience = YearsOfExperience.Create(command.MainInfo.YearsOfExperience).Value;
        var phoneNumber = PhoneNumber.Create(command.MainInfo.PhoneNumber).Value;
        
        var socialNetworks = command.UpdateSocialNetwork.SocialNetworks;
        var socialNetworkList = new List<SocialNetwork>();
        foreach (var socialNetwork in socialNetworks)
        {
            var value = SocialNetwork.Create(
                socialNetwork.NetworkName, 
                socialNetwork.NetworkAddress).Value;
            
            socialNetworkList.Add(value);
        }
        
        var requisitesForHelps = command.UpdateRequisitesForHelp.RequisitesForHelps;
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
            return Errors.Volunteer.AlreadyExist().ToErrorList();
        
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
        
        return volunteerToCreate.Value.Id.Value;
    }
}