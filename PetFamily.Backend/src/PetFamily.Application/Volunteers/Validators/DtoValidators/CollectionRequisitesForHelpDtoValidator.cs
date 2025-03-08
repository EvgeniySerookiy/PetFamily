using FluentValidation;
using PetFamily.Application.Volunteers.Actions.Create;
using PetFamily.Application.Volunteers.DTOs.Collections;
using PetFamily.Domain.PetManagement.VolunteerVO;

namespace PetFamily.Application.Volunteers.Validators.DtoValidators;

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