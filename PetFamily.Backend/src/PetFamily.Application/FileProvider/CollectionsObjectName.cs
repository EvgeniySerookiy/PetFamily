namespace PetFamily.Application.FileProvider;

public record CollectionsObjectName(
    IEnumerable<ObjectName> ObjectNames,
    string BucketName);