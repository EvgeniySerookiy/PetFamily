using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Database;
using PetFamily.Application.Extensions;
using PetFamily.Domain.PetManagement.AggregateRoot;
using PetFamily.Domain.PetManagement.SharedVO;
using PetFamily.Domain.PetManagement.VolunteerVO;
using PetFamily.Domain.Shared.ErrorContext;
using RequisitesForHelp = PetFamily.Domain.PetManagement.VolunteerVO.RequisitesForHelp;
using SocialNetwork = PetFamily.Domain.PetManagement.VolunteerVO.SocialNetwork;

namespace PetFamily.Application.PetManagement.Commands.Volunteers.AddVolunteer;

public class AddVolunteerHandler : ICommandHandler<Guid, AddVolunteerCommand>
{
    private readonly IVolunteersWriteRepository _volunteersWriteRepository;
    private readonly ILogger<AddVolunteerHandler> _logger;
    private readonly IValidator<AddVolunteerCommand> _validator;

    public AddVolunteerHandler(
        IVolunteersWriteRepository volunteersWriteRepository,
        ILogger<AddVolunteerHandler> logger,
        IValidator<AddVolunteerCommand> validator)
    {
        _volunteersWriteRepository = volunteersWriteRepository;
        _logger = logger;
        _validator = validator;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        AddVolunteerCommand command,
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
        
        var volunteer = await _volunteersWriteRepository.GetByEmail(email, cancellationToken);
        if (volunteer.IsFailure)
            return volunteer.Error.ToErrorList();
        
        var volunteerToCreate = Volunteer.Create(
            volunteerId, 
            fullName,
            email,
            description,
            yearsOfExperience,
            phoneNumber,
            TransferRequisitesForHelpsList.Create(requisitesForHelpList).Value,
            TransferSocialNetworkList.Create(socialNetworkList).Value);

        await _volunteersWriteRepository.Add(volunteerToCreate.Value, cancellationToken);
        
        _logger.LogInformation("Created volunteer with id: {volunteerId}", volunteerId);
        
        return volunteerToCreate.Value.Id.Value;
    }
}