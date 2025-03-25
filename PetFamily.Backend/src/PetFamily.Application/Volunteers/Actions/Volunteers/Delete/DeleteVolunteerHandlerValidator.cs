using FluentValidation;
using PetFamily.Application.Volunteers.Actions.Volunteers.Create;
using PetFamily.Domain.Shared.ErrorContext;

namespace PetFamily.Application.Volunteers.Actions.Volunteers.Delete;

public class DeleteVolunteerHandlerValidator : AbstractValidator<DeleteVolunteerRequest>
{
    public DeleteVolunteerHandlerValidator()
    {
        RuleFor(d => d.VolunteerId)
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired());
    }
}