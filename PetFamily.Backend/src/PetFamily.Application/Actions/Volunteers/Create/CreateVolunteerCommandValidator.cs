using FluentValidation;
using PetFamily.Application.Volunteers.Actions.Volunteers.Update.UpdateRequisitesForHelp;
using PetFamily.Application.Volunteers.Actions.Volunteers.Update.UpdateSocialNetwork;
using PetFamily.Application.Volunteers.Validators;

namespace PetFamily.Application.Volunteers.Actions.Volunteers.Create;

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