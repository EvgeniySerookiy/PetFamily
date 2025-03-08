using FluentValidation;
using PetFamily.Application.Volunteers.Actions.Create;
using PetFamily.Application.Volunteers.DTOs.Collections;
using PetFamily.Domain.PetManagement.VolunteerVO;

namespace PetFamily.Application.Volunteers.Validators.DtoValidators;

public class CollectionSocialNetworkDtoValidator : AbstractValidator<CollectionSocialNetworkDto>
{
    public CollectionSocialNetworkDtoValidator()
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