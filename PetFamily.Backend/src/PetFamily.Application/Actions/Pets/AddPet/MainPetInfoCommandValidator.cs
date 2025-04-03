using FluentValidation;
using PetFamily.Domain.PetManagement.PetVO;
using PetFamily.Domain.PetManagement.SharedVO;
using PetFamily.Domain.Shared.ErrorContext;

namespace PetFamily.Application.Volunteers.Actions.Pets.AddPet;

public class MainPetInfoCommandValidator : AbstractValidator<MainPetInfoCommand>
{
    public MainPetInfoCommandValidator()
    {
        RuleFor(m => m.Name).MustBeValueObject(PetName.Create);
        RuleFor(m => m.Title).MustBeValueObject(Title.Create);
        RuleFor(m => m.Description).MustBeValueObject(Description.Create);
        RuleFor(m => m.Color).MustBeValueObject(Color.Create);
        RuleFor(m => m.PetHealthInformation).MustBeValueObject(PetHealthInformation.Create);
        
        RuleFor(m => m.Address)
            .MustBeValueObject(a => Address.Create(
                a.Region, a.City, a.Street, a.Building, a.Apartment));
        
        RuleFor(m => m.PhoneNumber).MustBeValueObject(PhoneNumber.Create);

        RuleFor(m => m.PetSizeDto)
            .MustBeValueObject(p => Size.Create(
                p.Weight, p.Height));
        
        RuleFor(m => m.DateOfBirth)
            .Must(d => d == null || d <= DateTime.UtcNow)
            .WithError(Errors.General.ValueIsRequired("Date of birth"));
    }
}