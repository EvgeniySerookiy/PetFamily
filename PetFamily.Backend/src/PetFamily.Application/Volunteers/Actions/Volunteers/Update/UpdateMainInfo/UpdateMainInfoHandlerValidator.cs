using FluentValidation;
using PetFamily.Application.Volunteers.Actions.Create;
using PetFamily.Application.Volunteers.Validators;
using PetFamily.Domain.Shared.ErrorContext;

namespace PetFamily.Application.Volunteers.Actions.Update.UpdateMainInfo;

public class UpdateMainInfoHandlerValidator : AbstractValidator<UpdateMainInfoRequest>
{
    public UpdateMainInfoHandlerValidator()
    {
        RuleFor(u => u.VolunteerId)
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired());
        
        RuleFor(u => u.MainInfo)
            .SetValidator(new MainInfoDtoValidator());
    }
}