using FluentValidation;
using PetFamily.Application.Volunteers.DTOs;
using PetFamily.Domain.PetManagement.VolunteerVO;

namespace PetFamily.Application.Validation;

public class CreateVolunteerDtoValidator : AbstractValidator<CreateVolunteerDto>
{
    public CreateVolunteerDtoValidator()
    {
        RuleFor(c => new { c.FirstName, c.LastName, c.MiddleName })
            .MustBeValueObject(x => FullName.Create(
                x.FirstName, x.LastName, x.MiddleName));
    }
}