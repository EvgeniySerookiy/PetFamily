using PetFamily.Domain.PetContext.PetVO;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.VolunteerContext.VolunteerVO;

public record TransferSocialNetworkList
{
    private readonly List<SocialNetwork> _socialNetworks = new();
    public IReadOnlyList<SocialNetwork> SocialNetworks => _socialNetworks;
    
    private TransferSocialNetworkList() { }

    private TransferSocialNetworkList(IEnumerable<SocialNetwork> socialNetworks)
    {
        _socialNetworks = socialNetworks.ToList();
    }
    
    public void AddRequisitesForHelp(SocialNetwork socialNetwork)
    {
        _socialNetworks.Add(socialNetwork);
    }

    public static Result<TransferSocialNetworkList> Create(IEnumerable<SocialNetwork> socialNetworks)
    {
        return new TransferSocialNetworkList(socialNetworks);
    }
}