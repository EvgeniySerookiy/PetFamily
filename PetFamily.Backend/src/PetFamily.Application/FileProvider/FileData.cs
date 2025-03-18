namespace PetFamily.Infrastructure.Models;

public record  FileData(
    Stream Stream, 
    string BucketName, 
    string ObjectName);