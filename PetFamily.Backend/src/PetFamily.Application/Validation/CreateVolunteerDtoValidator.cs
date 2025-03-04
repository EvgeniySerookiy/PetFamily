using FluentValidation;
using PetFamily.Application.Volunteers.DTOs;
using PetFamily.Domain.PetManagement.SharedVO;
using PetFamily.Domain.PetManagement.VolunteerVO;

namespace PetFamily.Application.Validation;

public class CreateVolunteerDtoValidator : AbstractValidator<CreateVolunteerDto>
{
    public CreateVolunteerDtoValidator()
    {
        RuleFor(c => c.FullName)
            .MustBeValueObject(x => FullName.Create(
                x.FirstName, x.LastName, x.MiddleName));
 
        RuleFor(c => c.Email).MustBeValueObject(Email.Create);
        RuleFor(c => c.Description).MustBeValueObject(Description.Create);
        RuleFor(c => c.YearsOfExperience).MustBeValueObject(YearsOfExperience.Create);
        RuleFor(c => c.PhoneNumber).MustBeValueObject(PhoneNumber.Create);

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

        RuleForEach(c => c.RequisitesForHelps).ChildRules(requisitesForHelps =>
        {
            requisitesForHelps.RuleFor(x => new
                {
                    x.Recipient,
                    x.PaymentDetails
                })
                .MustBeValueObject(x => RequisitesForHelp.Create(
                    x.Recipient,
                    x.PaymentDetails));
        });
    }
}