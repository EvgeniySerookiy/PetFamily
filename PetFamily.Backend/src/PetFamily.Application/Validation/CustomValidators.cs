using CSharpFunctionalExtensions;
using FluentValidation;
using PetFamily.Domain.Shared.ErrorContext;

namespace PetFamily.Application.Validation;

public static class CustomValidators
{
    public static IRuleBuilderOptionsConditions< T, TElement> MustBeValueObject<T, TElement, TValueObject>(
            this IRuleBuilder<T, TElement> ruleBuilder,
            Func<TElement, Result<TValueObject, Error>> factoryMethod)
    {
        return ruleBuilder.Custom((value, context) =>
        {
            Result<TValueObject, Error> result = factoryMethod(value);
            
            if(result.IsFailure)
                return;
            
            context.AddFailure(result.Error.Message);
        });
    }
}