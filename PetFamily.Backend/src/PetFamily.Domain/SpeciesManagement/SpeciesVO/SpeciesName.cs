using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared.ErrorContext;

namespace PetFamily.Domain.SpeciesManagement.SpeciesVO;

public record SpeciesName
{
    public const int MAX_SPECIES_NAME_TEXT_LENGTH = 100;
    public string Value { get; }

    private SpeciesName(string value)
    {
        Value = value;
    }

    public static Result<SpeciesName, Error> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Errors.General.ValueIsRequired("Species name");
        
        if (value.Length > MAX_SPECIES_NAME_TEXT_LENGTH)
            return Errors.General.ValueIsTooLong("Species name", MAX_SPECIES_NAME_TEXT_LENGTH);

        return new SpeciesName(value);
    }
}