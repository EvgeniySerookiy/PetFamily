using FluentValidation;
using PetFamily.Application.Volunteers.Create;
using PetFamily.Application.Volunteers.Requests;
using PetFamily.Domain.Shared.ErrorContext;

namespace PetFamily.Application.Volunteers.Validators;

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