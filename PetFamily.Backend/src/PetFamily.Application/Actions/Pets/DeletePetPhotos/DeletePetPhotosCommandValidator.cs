using FluentValidation;
using PetFamily.Domain.Shared.ErrorContext;

namespace PetFamily.Application.Volunteers.Actions.Pets.DeletePetPhotos;

public class DeletePetPhotosCommandValidator : AbstractValidator<DeletePetPhotosCommand>
{
    public DeletePetPhotosCommandValidator()
    {
        RuleFor(d => d.PhotosId).NotEmpty().WithError(Errors.General.ValueIsRequired());
        
        RuleForEach(c => c.PhotosId)
            .NotEmpty().WithError(Errors.General.ValueIsRequired())
            .NotEqual(Guid.Empty).WithError(Errors.General.ValueIsRequired());
    }
}