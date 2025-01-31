using CSharpFunctionalExtensions;

namespace PetFamily.Domain.VolunteerVO;

public record SocialNetwork
{
    public string NetworkName { get; }
    public string NetworkAddress { get; }

    private SocialNetwork(string networkName, string networkAddress)
    {
        NetworkName = networkName;
        NetworkAddress = networkAddress;
    }

    public static Result<SocialNetwork> Create(string networkName, string networkAddress)
    {
        if (string.IsNullOrWhiteSpace(networkName))
        {
            return Result.Failure<SocialNetwork>("Network name cannot be null or empty.");
        }
        
        if (string.IsNullOrWhiteSpace(networkAddress))
        {
            return Result.Failure<SocialNetwork>("Network address cannot be null or empty.");
        }
        
        var socialNetwork = new SocialNetwork(networkName, networkAddress);
        
        return Result.Success(socialNetwork);
    }
}