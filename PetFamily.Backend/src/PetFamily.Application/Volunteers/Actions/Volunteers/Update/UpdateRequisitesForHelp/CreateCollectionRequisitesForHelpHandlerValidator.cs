using FluentValidation;
using PetFamily.Application.Volunteers.Actions.Volunteers.Create;
using PetFamily.Application.Volunteers.Validators;
using PetFamily.Domain.Shared.ErrorContext;

namespace PetFamily.Application.Volunteers.Actions.Volunteers.Update.UpdateRequisitesForHelp;

public class
    CreateCollectionRequisitesForHelpHandlerValidator : AbstractValidator<UpdateCollectionRequisitesForHelpRequest>
{
    public CreateCollectionRequisitesForHelpHandlerValidator()
    {
        RuleFor(u => u.VolunteerId)
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired());

        RuleFor(c => c.CollectionRequisitesForHelp)
            .SetValidator(new CollectionRequisitesForHelpDtoValidator());
    }
}