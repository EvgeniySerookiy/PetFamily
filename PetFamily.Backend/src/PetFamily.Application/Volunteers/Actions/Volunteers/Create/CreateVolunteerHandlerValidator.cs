using FluentValidation;
using PetFamily.Application.Volunteers.Validators;

namespace PetFamily.Application.Volunteers.Actions.Volunteers.Create;

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