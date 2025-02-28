using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared.ErrorContext;

namespace PetFamily.Domain.PetManagement.VolunteerVO;

public record SocialNetwork
{
    public const int MAX_NETWORK_NAME_TEXT_LENGTH = 100;
    public const int MAX_NETWORK_ADDRESS_TEXT_LENGTH = 200;
    public string NetworkName { get; }
    public string NetworkAddress { get; }

    private SocialNetwork(string networkName, string networkAddress)
    {
        NetworkName = networkName;
        NetworkAddress = networkAddress;
    }

    public static Result<SocialNetwork, Error> Create(string networkName, string networkAddress)
    {
        if (string.IsNullOrWhiteSpace(networkName))
            return Errors.General.ValueIsRequired("Network name");
        
        if (string.IsNullOrWhiteSpace(networkAddress))
            return Errors.General.ValueIsRequired("Network address");
        
        if(networkName.Length > MAX_NETWORK_NAME_TEXT_LENGTH)
            return Errors.General.ValueIsTooLong("Network name", MAX_NETWORK_NAME_TEXT_LENGTH);
        
        if(networkAddress.Length > MAX_NETWORK_ADDRESS_TEXT_LENGTH)
            return Errors.General.ValueIsTooLong("Network address", MAX_NETWORK_ADDRESS_TEXT_LENGTH);
        
        return new SocialNetwork(networkName, networkAddress);
    }
}