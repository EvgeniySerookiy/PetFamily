using FluentValidation;
using PetFamily.Domain.Shared.ErrorContext;

namespace PetFamily.Application.PetManagement.Commands.Volunteers.MovePets;

public class MovePetsCommandValidator : AbstractValidator<MovePetsCommand>
{
    public MovePetsCommandValidator()
    {
        RuleFor(a => a.Id)
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired());
        
        RuleFor(m => m.CurrentPosition).NotNull().GreaterThan(0);
        RuleFor(m => m.ToPosition).NotNull().GreaterThan(0);
    }
}