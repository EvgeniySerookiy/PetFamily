namespace PetFamily.Domain.Shared.ErrorContext;

public static class Errors
{
    public static class General
    {
        public static Error ValueIsRequired(string? name = null)
        {
            var label = name ?? "value";
            return Error.Validation("value.is.invalid", $"{label} is invalid");
        }
        
        public static Error ValueIsTooLong(string? name = null, int maxLength = 0)
        {
            var label = name ?? "value";
            return Error.Validation("value.is.too.long", $"{label} is too long. Max length is {maxLength}.");
        }
        
        public static Error ValueCannotBeNegative(string? name = null, int? value = null)
        {
            var label = name ?? "value";
            var valueInfo = value.HasValue ? $" Value is {value}." : "";
            return Error.Validation("value.cannot.be.negative", $"{label} cannot be negative.{valueInfo}");
        }

        
        public static Error NotFound(Guid? id = null)
        {
            var forId = id == null ? "" : $"for Id {id}";
            return Error.NotFound("record.not.found", $"record not found {forId}");
        }
    }
}