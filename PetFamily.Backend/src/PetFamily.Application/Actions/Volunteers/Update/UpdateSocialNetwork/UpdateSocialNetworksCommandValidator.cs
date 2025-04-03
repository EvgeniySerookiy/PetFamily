using FluentValidation;
using PetFamily.Domain.PetManagement.VolunteerVO;

namespace PetFamily.Application.Volunteers.Actions.Volunteers.Update.UpdateSocialNetwork;

public class UpdateSocialNetworksCommandValidator : AbstractValidator<UpdateSocialNetworksCommand>
{
    public UpdateSocialNetworksCommandValidator()
    {
        RuleForEach(c => c.SocialNetworks).ChildRules(socialNetwork =>
        {
            socialNetwork.RuleFor(x => new
                {
                    x.NetworkName, 
                    x.NetworkAddress
                })
                .MustBeValueObject(x => SocialNetwork.Create(
                    x.NetworkName, 
                    x.NetworkAddress));
        });
    }
}