using FluentValidation;
using PetFamily.Domain.PetManagement.VolunteerVO;
using PetFamily.Domain.Shared.ErrorContext;

namespace PetFamily.Application.PetManagement.Commands.Volunteers.UpdateVolunteer.UpdateRequisitesForHelp;

public class UpdateRequisitesForHelpValidator : AbstractValidator<UpdateRequisitesForHelpCommand>
{
    public UpdateRequisitesForHelpValidator()
    {
        RuleFor(u => u.Id)
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired());
        
        RuleForEach(u => u.RequisitesForHelps).ChildRules(requisitesForHelps =>
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