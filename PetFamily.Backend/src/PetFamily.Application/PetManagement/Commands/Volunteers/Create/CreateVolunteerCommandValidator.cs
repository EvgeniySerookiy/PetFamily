using FluentValidation;
using PetFamily.Application.Dtos.Validators;
using PetFamily.Application.PetManagement.Commands.Volunteers.Update.UpdateRequisitesForHelp;
using PetFamily.Application.PetManagement.Commands.Volunteers.Update.UpdateSocialNetwork;

namespace PetFamily.Application.PetManagement.Commands.Volunteers.Create;

public class CreateVolunteerCommandValidator : AbstractValidator<CreateVolunteerCommand>
{
    public CreateVolunteerCommandValidator()
    {
        RuleFor(c => c.MainInfo)
            .SetValidator(new MainInfoDtoValidator());

        RuleFor(c => c.UpdateSocialNetwork)
            .SetValidator(new UpdateSocialNetworksCommandValidator());
        
        RuleFor(c => c.UpdateRequisitesForHelp)
            .SetValidator(new UpdateRequisitesForHelpCommandValidator());
    }
}