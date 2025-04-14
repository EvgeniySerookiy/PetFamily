using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared.ErrorContext;

namespace PetFamily.Domain.SpeciesManagement.SpeciesVO;

public record BreedName
{
    public const int MAX_BREED_NAME_TEXT_LENGTH = 100;
    public string Value { get; }

    private BreedName(string value)
    {
        Value = value;
    }

    public static Result<BreedName, Error> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Errors.General.ValueIsRequired("Breed name");
        
        if (value.Length > MAX_BREED_NAME_TEXT_LENGTH)
            return Errors.General.ValueIsTooLong("Breed name", MAX_BREED_NAME_TEXT_LENGTH);

        return new BreedName(value);
    }
}