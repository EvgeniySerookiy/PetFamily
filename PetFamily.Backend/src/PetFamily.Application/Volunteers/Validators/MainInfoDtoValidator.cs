using FluentValidation;
using PetFamily.Application.Volunteers.Actions.Create;
using PetFamily.Application.Volunteers.DTOs;
using PetFamily.Domain.PetManagement.SharedVO;
using PetFamily.Domain.PetManagement.VolunteerVO;

namespace PetFamily.Application.Volunteers.Validators;

public class MainInfoDtoValidator : AbstractValidator<MainInfoDto>
{
    public MainInfoDtoValidator()
    {
        RuleFor(m => m.FullName)
            .MustBeValueObject(f => FullName.Create(
                f.FirstName, f.LastName, f.MiddleName));
        
        RuleFor(m => m.Email).MustBeValueObject(Email.Create);
        RuleFor(m => m.Description).MustBeValueObject(Description.Create);
        RuleFor(m => m.YearsOfExperience).MustBeValueObject(YearsOfExperience.Create);
        RuleFor(m => m.PhoneNumber).MustBeValueObject(PhoneNumber.Create);
    }
}