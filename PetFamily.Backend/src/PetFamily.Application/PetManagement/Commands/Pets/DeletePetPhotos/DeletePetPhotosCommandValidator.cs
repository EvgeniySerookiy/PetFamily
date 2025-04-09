using FluentValidation;
using PetFamily.Domain.Shared.ErrorContext;

namespace PetFamily.Application.PetManagement.Commands.Pets.DeletePetPhotos;

public class DeletePetPhotosCommandValidator : AbstractValidator<DeletePetPhotosCommand>
{
    public DeletePetPhotosCommandValidator()
    {
        RuleFor(a => a.VolunteerId)
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired());
        
        RuleFor(a => a.PetId)
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired());
        
        RuleFor(d => d.PhotosId).NotEmpty().WithError(Errors.General.ValueIsRequired());
        
        RuleForEach(c => c.PhotosId)
            .NotEmpty().WithError(Errors.General.ValueIsRequired())
            .NotEqual(Guid.Empty).WithError(Errors.General.ValueIsRequired());
    }
}