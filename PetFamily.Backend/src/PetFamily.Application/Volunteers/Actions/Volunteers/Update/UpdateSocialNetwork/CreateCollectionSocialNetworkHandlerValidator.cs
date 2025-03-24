using FluentValidation;
using PetFamily.Application.Volunteers.Actions.Volunteers.Create;
using PetFamily.Application.Volunteers.Validators;
using PetFamily.Domain.Shared.ErrorContext;

namespace PetFamily.Application.Volunteers.Actions.Volunteers.Update.UpdateSocialNetwork;

public class CreateCollectionSocialNetworkHandlerValidator : AbstractValidator<UpdateCollectionSocialNetworkRequest>
{
    public CreateCollectionSocialNetworkHandlerValidator()
    {
        RuleFor(u => u.VolunteerId)
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired());

        RuleFor(c => c.CollectionSocialNetwork)
            .SetValidator(new CollectionSocialNetworkDtoValidator());
    }
}