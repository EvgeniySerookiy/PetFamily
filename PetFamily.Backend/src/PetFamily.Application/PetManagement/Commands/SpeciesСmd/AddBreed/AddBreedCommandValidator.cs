using FluentValidation;
using PetFamily.Domain.SpeciesManagement.SpeciesVO;

namespace PetFamily.Application.PetManagement.Commands.SpeciesСmd.AddBreed;

public class AddBreedCommandValidator : AbstractValidator<AddBreedCommand>
{
    public AddBreedCommandValidator()
    {
        RuleFor(a => a.Name).MustBeValueObject(BreedName.Create);
    }
}