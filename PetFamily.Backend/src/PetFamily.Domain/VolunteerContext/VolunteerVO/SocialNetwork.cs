using PetFamily.Domain.Shared;
using PetFamily.Domain.SharedVO;

namespace PetFamily.Domain.VolunteerContext.VolunteerVO;

public record SocialNetwork
{
    public NotEmptyString NetworkName { get; }
    public Description NetworkAddress { get; }
    
    private SocialNetwork() {}

    private SocialNetwork(NotEmptyString networkName, Description networkAddress)
    {
        NetworkName = networkName;
        NetworkAddress = networkAddress;
    }

    public static Result<SocialNetwork> Create(NotEmptyString networkName, Description networkAddress)
    {
        var socialNetwork = new SocialNetwork(networkName, networkAddress);
        
        return socialNetwork;
    }
}