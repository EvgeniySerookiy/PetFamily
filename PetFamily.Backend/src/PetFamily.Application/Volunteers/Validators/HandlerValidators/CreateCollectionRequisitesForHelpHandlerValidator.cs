using FluentValidation;
using PetFamily.Application.Volunteers.Actions.Create;
using PetFamily.Application.Volunteers.Requests;
using PetFamily.Application.Volunteers.Validators.DtoValidators;
using PetFamily.Domain.Shared.ErrorContext;

namespace PetFamily.Application.Volunteers.Validators.HandlerValidators;

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