using FluentValidation;
using PetFamily.Application.Volunteers.Actions.Create;
using PetFamily.Domain.Shared.ErrorContext;

namespace PetFamily.Application.Volunteers.Actions.Delete;

public class DeleteVolunteerHandlerValidator : AbstractValidator<DeleteVolunteerRequest>
{
    public DeleteVolunteerHandlerValidator()
    {
        RuleFor(d => d.VolunteerId)
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired());
    }
}