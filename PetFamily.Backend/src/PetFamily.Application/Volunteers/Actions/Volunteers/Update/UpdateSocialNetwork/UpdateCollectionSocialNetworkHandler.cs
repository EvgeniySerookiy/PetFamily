using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Domain.PetManagement.VolunteerVO;
using PetFamily.Domain.Shared.ErrorContext;

namespace PetFamily.Application.Volunteers.Actions.Update.UpdateSocialNetwork;

public class UpdateCollectionSocialNetworkHandler
{
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly ILogger<UpdateCollectionSocialNetworkHandler> _logger;

    public UpdateCollectionSocialNetworkHandler(
        IVolunteersRepository volunteersRepository,
        ILogger<UpdateCollectionSocialNetworkHandler> logger)
    {
        _volunteersRepository = volunteersRepository;
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

        var result = await _volunteersRepository.Save(volunteerResult.Value, cancellationToken);
        
        _logger.LogInformation(
            "Update volunteer {socialNetworkList} with id {volunteerId}", 
            socialNetworkList,
            request.VolunteerId);

        return result;
    }
}