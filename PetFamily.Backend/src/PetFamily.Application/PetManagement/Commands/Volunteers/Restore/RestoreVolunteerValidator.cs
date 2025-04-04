using FluentValidation;
using PetFamily.Domain.Shared.ErrorContext;

namespace PetFamily.Application.PetManagement.Commands.Volunteers.Restore;

public class RestoreVolunteerValidator : AbstractValidator<RestoreVolunteerCommand>
{
    public RestoreVolunteerValidator()
    {
        RuleFor(r => r.Id)
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired());
    }
}