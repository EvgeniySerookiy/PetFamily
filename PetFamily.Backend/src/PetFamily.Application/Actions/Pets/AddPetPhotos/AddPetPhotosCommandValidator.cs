using FluentValidation;
using PetFamily.Application.Volunteers.Validators;

namespace PetFamily.Application.Volunteers.Actions.Pets.AddPetPhotos;

public class AddPetPhotosCommandValidator : AbstractValidator<AddPetPhotosCommand>
{
    public AddPetPhotosCommandValidator()
    {
        RuleForEach(a => a.Photos).SetValidator(new CreatePhotoDtoValidator());
    }
}