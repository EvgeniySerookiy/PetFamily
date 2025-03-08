using FluentValidation;
using PetFamily.Application.Volunteers.Actions.Create;
using PetFamily.Application.Volunteers.Requests;
using PetFamily.Domain.Shared.ErrorContext;

namespace PetFamily.Application.Volunteers.Validators.HandlerValidators;

public class DeleteVolunteerHandlerValidator : AbstractValidator<DeleteVolunteerRequest>
{
    public DeleteVolunteerHandlerValidator()
    {
        RuleFor(d => d.VolunteerId)
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired());
    }
}