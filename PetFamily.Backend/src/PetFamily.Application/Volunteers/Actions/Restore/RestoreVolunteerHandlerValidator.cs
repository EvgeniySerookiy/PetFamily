using FluentValidation;
using PetFamily.Application.Volunteers.Actions.Create;
using PetFamily.Domain.Shared.ErrorContext;

namespace PetFamily.Application.Volunteers.Actions.Restore;

public class RestoreVolunteerHandlerValidator : AbstractValidator<RestoreVolunteerRequest>
{
    public RestoreVolunteerHandlerValidator()
    {
        RuleFor(d => d.VolunteerId)
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired());
    }
}