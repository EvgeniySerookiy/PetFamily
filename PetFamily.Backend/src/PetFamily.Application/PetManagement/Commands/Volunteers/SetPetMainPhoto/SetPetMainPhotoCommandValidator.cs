using FluentValidation;
using PetFamily.Domain.Shared.ErrorContext;

namespace PetFamily.Application.PetManagement.Commands.Volunteers.SetPetMainPhoto;

public class SetPetMainPhotoCommandValidator : AbstractValidator<SetPetMainPhotoCommand>
{
    public SetPetMainPhotoCommandValidator()
    {
        RuleFor(s => s.PhotoPath)
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired());
    }
}