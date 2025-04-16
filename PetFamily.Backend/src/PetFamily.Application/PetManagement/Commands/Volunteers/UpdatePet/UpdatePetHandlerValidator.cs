using FluentValidation;
using PetFamily.Domain.PetManagement.PetVO;
using PetFamily.Domain.PetManagement.SharedVO;
using PetFamily.Domain.Shared.ErrorContext;

namespace PetFamily.Application.PetManagement.Commands.Volunteers.UpdatePet;

public class UpdatePetHandlerValidator : AbstractValidator<UpdatePetCommand>
{
    public UpdatePetHandlerValidator()
    {
        RuleFor(u => u.Name).MustBeValueObject(PetName.Create);
        RuleFor(u => u.Title).MustBeValueObject(Title.Create);
        RuleFor(u => u.Description).MustBeValueObject(Description.Create);
        RuleFor(u => u.Color).MustBeValueObject(Color.Create);
        RuleFor(u => u.PetHealthInformation).MustBeValueObject(PetHealthInformation.Create);
        
        RuleFor(u => u.Address)
            .MustBeValueObject(a => Address.Create(
                a.Region, a.City, a.Street, a.Building, a.Apartment));
        
        RuleFor(u => u.PhoneNumber).MustBeValueObject(PhoneNumber.Create);
        
        RuleFor(u => u.PetSizeDto).MustBeValueObject(p => Size.Create(
            p.Weight, p.Height));
        
        RuleFor(u => u.DateOfBirth)
            .Must(d => d == null || d <= DateTime.UtcNow)
            .WithError(Errors.General.ValueIsRequired("Date of birth"));
    }
}