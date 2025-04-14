using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Database;
using PetFamily.Application.Extensions;
using PetFamily.Domain.PetManagement.VolunteerVO;
using PetFamily.Domain.Shared.ErrorContext;

namespace PetFamily.Application.PetManagement.Commands.Volunteers.UpdateVolunteer.UpdateSocialNetwork;

public class UpdateSocialNetworkHandler : ICommandHandler<Guid, UpdateSocialNetworksCommand>
{
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly ILogger<UpdateSocialNetworkHandler> _logger;
    private readonly IValidator<UpdateSocialNetworksCommand> _validator;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateSocialNetworkHandler(
        IVolunteersRepository volunteersRepository,
        ILogger<UpdateSocialNetworkHandler> logger,
        IValidator<UpdateSocialNetworksCommand> validator,
        IUnitOfWork unitOfWork)
    {
        _volunteersRepository = volunteersRepository;
        _logger = logger;
        _validator = validator;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        UpdateSocialNetworksCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToErrorList();
        
        var volunteerResult = await _volunteersRepository.GetById(command.Id, cancellationToken);
        if (volunteerResult.IsFailure)
            return volunteerResult.Error.ToErrorList();

        var socialNetworks = command.SocialNetworks;
        var socialNetworkList = new List<SocialNetwork>();
        foreach (var socialNetwork in socialNetworks)
        {
            var value = SocialNetwork.Create(
                socialNetwork.NetworkName,
                socialNetwork.NetworkAddress).Value;

            socialNetworkList.Add(value);
        }

        volunteerResult.Value.UpdateSocialNetworkList(
            socialNetworkList);

        await _unitOfWork.SaveChanges(cancellationToken);
        
        _logger.LogInformation(
            "Update volunteer {socialNetworkList} with id {volunteerId}", 
            socialNetworkList,
            command.Id);

        return volunteerResult.Value.Id.Value;
    }
}