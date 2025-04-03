using FluentValidation;
using PetFamily.Domain.PetManagement.VolunteerVO;

namespace PetFamily.Application.Volunteers.Actions.Volunteers.Update.UpdateRequisitesForHelp;

public class UpdateRequisitesForHelpCommandValidator : AbstractValidator<UpdateRequisitesForHelpCommand>
{
    public UpdateRequisitesForHelpCommandValidator()
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