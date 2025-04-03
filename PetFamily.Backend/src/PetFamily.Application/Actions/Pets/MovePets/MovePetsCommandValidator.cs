using FluentValidation;

namespace PetFamily.Application.Volunteers.Actions.Pets.MovePets;

public class MovePetsCommandValidator : AbstractValidator<MovePetsCommand>
{
    public MovePetsCommandValidator()
    {
        RuleFor(m => m.CurrentPosition).NotNull().GreaterThan(0);
        RuleFor(m => m.ToPosition).NotNull().GreaterThan(0);
    }
}