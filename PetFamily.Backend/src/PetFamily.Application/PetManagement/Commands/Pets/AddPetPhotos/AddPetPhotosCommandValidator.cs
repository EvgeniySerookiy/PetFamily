using FluentValidation;
using PetFamily.Application.Dtos.Validators;
using PetFamily.Domain.Shared.ErrorContext;

namespace PetFamily.Application.PetManagement.Commands.Pets.AddPetPhotos;

public class AddPetPhotosCommandValidator : AbstractValidator<AddPetPhotosCommand>
{
    public AddPetPhotosCommandValidator()
    {
        RuleFor(a => a.VolunteerId)
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired());
        
        RuleFor(a => a.PetId)
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired());
        
        RuleForEach(a => a.Photos).SetValidator(new CreatePhotoDtoValidator());
    }
}