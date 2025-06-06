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
            return Error.Validation(
                "value.is.too.long", $"{label} is too long. Max length is {maxLength}.");
        }
        
        public static Error OutOfRange(int value)
        {
            return Error.Validation(
                "out.of.range", $"out of range {value}");
        }
        
        public static Error InvalidRequest(int value)
        {
            return Error.Validation(
                "invalid.request", $"invalid request {value}");
        }

        public static Error ValueCannotBeNegative(string? name = null, double? value = null)
        {
            var label = name ?? "value";
            var valueInfo = value.HasValue ? $" Value is {value}." : "";
            return Error.Validation(
                "value.cannot.be.negative", $"{label} cannot be negative.{valueInfo}");
        }

        public static Error NotFound(Guid? id = null)
        {
            var forId = id == null ? "" : $"for Id {id}";
            return Error.NotFound(
                "record.not.found", $"record not found {forId}");
        }
    }

    public static class Volunteer
    {
        public static Error AlreadyExist()
        {
            return Error.Validation("record.already.exist", "Volunteer already exist");
        }
        
        public static Error NotFound(Guid? id = null)
        {
            var forId = id == null ? "" : $"for Id {id}";
            return Error.NotFound(
                "record.not.found", $"record not found {forId}");
        }
    }
    
    public static class Pet
    {
        public static Error AlreadyExist()
        {
            return Error.Validation("record.already.exist", "Pet already exist");
        }
        
        public static Error NotFound(Guid? id = null)
        {
            var forId = id == null ? "" : $"for Id {id}";
            return Error.NotFound(
                "record.not.found", $"record not found {forId}");
        }
    }
    
    public static class Species
    {
        public static Error AlreadyExist()
        {
            return Error.Validation("record.already.exist", "Species already exist");
        }
        
        public static Error NotFound(Guid? id = null)
        {
            var forId = id == null ? "" : $"for Id {id}";
            return Error.NotFound(
                "record.not.found", $"record not found {forId}");
        }
        
        public static Error NotFoundByName(string? name = null)
        {
            var forName = string.IsNullOrWhiteSpace(name) ? "" : $"for name '{name}'";
            return Error.NotFound(
                "record.not.found.by.name", $"Record not found {forName}");
        }
    }
    
    public static class Breed
    {
        public static Error AlreadyExist()
        {
            return Error.Validation("record.already.exist", "Breed already exist");
        }
        
        public static Error NotFound(Guid? id = null)
        {
            var forId = id == null ? "" : $"for Id {id}";
            return Error.NotFound(
                "record.not.found", $"record not found {forId}");
        }
    }
}