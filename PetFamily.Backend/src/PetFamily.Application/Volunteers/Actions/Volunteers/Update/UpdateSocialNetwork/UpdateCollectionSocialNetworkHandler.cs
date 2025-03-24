using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Database;
using PetFamily.Domain.PetManagement.VolunteerVO;
using PetFamily.Domain.Shared.ErrorContext;

namespace PetFamily.Application.Volunteers.Actions.Volunteers.Update.UpdateSocialNetwork;

public class UpdateCollectionSocialNetworkHandler
{
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UpdateCollectionSocialNetworkHandler> _logger;

    public UpdateCollectionSocialNetworkHandler(
        IVolunteersRepository volunteersRepository,
        IUnitOfWork unitOfWork,
        ILogger<UpdateCollectionSocialNetworkHandler> logger)
    {
        _volunteersRepository = volunteersRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<Guid, Error>> Handle(
        UpdateCollectionSocialNetworkRequest request,
        CancellationToken cancellationToken = default)
    {
        var volunteerResult = await _volunteersRepository.GetById(request.VolunteerId, cancellationToken);
        if (volunteerResult.IsFailure)
            return volunteerResult.Error;

        var socialNetworks = request.CollectionSocialNetwork.SocialNetworks;
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
            request.VolunteerId);

        return volunteerResult.Value.Id.Value;
    }
}