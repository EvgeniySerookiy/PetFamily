using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared.ErrorContext;
using PetFamily.Infrastructure.Models;

namespace PetFamily.Application.Providers;

public interface IFileProvider
{
    Task<Result<string, Error>> UploadFile(
        FileData fileData,
        CancellationToken cancellationToken = default);
    
    Task<Result<string, Error>> DeleteFile(
        Guid fieldId,
        CancellationToken cancellationToken = default);
    
    Task<Result<string, Error>> GetFileDownload(
        Guid fieldId);
}