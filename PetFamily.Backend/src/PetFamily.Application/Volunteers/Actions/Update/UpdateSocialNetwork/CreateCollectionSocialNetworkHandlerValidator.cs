using FluentValidation;
using PetFamily.Application.Volunteers.Actions.Create;
using PetFamily.Application.Volunteers.Actions.Update.UpdateSocialNetwork;
using PetFamily.Domain.Shared.ErrorContext;

namespace PetFamily.Application.Volunteers.Validators.HandlerValidators;

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