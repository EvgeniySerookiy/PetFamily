using FluentValidation;
using PetFamily.Application.Dtos.Validators;
using PetFamily.Application.PetManagement.Commands.Volunteers.UpdateVolunteer.UpdateRequisitesForHelp;
using PetFamily.Application.PetManagement.Commands.Volunteers.UpdateVolunteer.UpdateSocialNetwork;

namespace PetFamily.Application.PetManagement.Commands.Volunteers.CreateVolunteer;

public class AddVolunteerCommandValidator : AbstractValidator<AddVolunteerCommand>
{
    public AddVolunteerCommandValidator()
    {
        RuleFor(c => c.MainInfo)
            .SetValidator(new MainInfoDtoValidator());

        RuleFor(c => c.UpdateSocialNetwork)
            .SetValidator(new UpdateSocialNetworksValidator());
        
        RuleFor(c => c.UpdateRequisitesForHelp)
            .SetValidator(new UpdateRequisitesForHelpValidator());
    }
}