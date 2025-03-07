using FluentValidation;
using PetFamily.Application.Volunteers.Create;
using PetFamily.Application.Volunteers.DTOs.Collections;
using PetFamily.Domain.PetManagement.VolunteerVO;

namespace PetFamily.Application.Volunteers.Validators;

public class CollectionRequisitesForHelpDtoValidator : AbstractValidator<CollectionRequisitesForHelpDto>
{
    public CollectionRequisitesForHelpDtoValidator()
    {
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