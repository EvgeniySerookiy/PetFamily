using FluentValidation;
using PetFamily.Application.Volunteers.Actions.Create;
using PetFamily.Application.Volunteers.Actions.Update.UpdateRequisitesForHelp;
using PetFamily.Application.Volunteers.Actions.Update.UpdateSocialNetwork;

namespace PetFamily.Application.Volunteers.Validators.HandlerValidators;

public class CreateVolunteerHandlerValidator : AbstractValidator<CreateVolunteerRequest>
{
    public CreateVolunteerHandlerValidator()
    {
        RuleFor(c => c.MainInfo)
            .SetValidator(new MainInfoDtoValidator());

        RuleFor(c => c.CollectionSocialNetwork)
            .SetValidator(new CollectionSocialNetworkDtoValidator());
        
        RuleFor(c => c.CollectionRequisitesForHelp)
            .SetValidator(new CollectionRequisitesForHelpDtoValidator());
    }
}