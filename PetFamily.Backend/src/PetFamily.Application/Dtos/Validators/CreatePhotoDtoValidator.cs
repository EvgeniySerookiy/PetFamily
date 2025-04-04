using FluentValidation;
using PetFamily.Application.Dtos.PetDTOs;
using PetFamily.Domain.Shared.ErrorContext;

namespace PetFamily.Application.Dtos.Validators;

public class CreatePhotoDtoValidator : AbstractValidator<CreatePhotoDto>
{
    public CreatePhotoDtoValidator()
    {
        RuleFor(c => c.Content)
            .Must(c => c.CanSeek && c.Length <= 5_000_000)
            .WithError(Errors.General.ValueIsRequired());
        RuleFor(c => c.PhotoName).NotEmpty().WithError(Errors.General.ValueIsRequired());
    }
}