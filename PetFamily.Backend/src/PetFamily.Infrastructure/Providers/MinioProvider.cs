using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using Minio;
using Minio.DataModel.Args;
using PetFamily.Application.Providers;
using PetFamily.Domain.Shared.ErrorContext;
using PetFamily.Infrastructure.Models;

namespace PetFamily.Infrastructure.Providers;

public class MinioProvider : IFileProvider
{
    private readonly IMinioClient _minioClient;
    private readonly ILogger<MinioProvider> _logger;

    public MinioProvider(IMinioClient minioClient, ILogger<MinioProvider> logger)
    {
        _minioClient = minioClient;
        _logger = logger;
    }

    public async Task<Result<string, Error>> UploadFile(
        FileData fileData,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var buketExistArgs = new BucketExistsArgs()
                .WithBucket("photos");

            var bucketExist = await _minioClient.BucketExistsAsync(buketExistArgs, cancellationToken);

            if (bucketExist == false)
            {
                var makeBuketArgs = new MakeBucketArgs()
                    .WithBucket("photos");

                await _minioClient.MakeBucketAsync(makeBuketArgs, cancellationToken);
            }

            var path = Guid.NewGuid();

            var putObjectsArgs = new PutObjectArgs()
                .WithBucket("photos")
                .WithStreamData(fileData.Stream)
                .WithObjectSize(fileData.Stream.Length)
                .WithObject(path.ToString());

            var result = await _minioClient.PutObjectAsync(putObjectsArgs, cancellationToken);

            return result.ObjectName;
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Fail to upload file in minio");
            return Error.Failure("file.upload", "Fail to upload file in minio");
        }
    }

    public async Task<Result<string, Error>> DeleteFile(
        Guid fieldId, 
        CancellationToken cancellationToken = default)
    {
        try
        {
            var removeObjectArgs = new RemoveObjectArgs()
                .WithBucket("photos")
                .WithObject(fieldId.ToString());
            
            await _minioClient.RemoveObjectAsync(removeObjectArgs, cancellationToken);
            
            return Result.Success<string, Error>(fieldId.ToString());
            
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Fail to upload file in minio");
            return Error.Failure("file.upload", "Fail to upload file in minio");
        }
    }
    
    public async Task<Result<string, Error>> GetFileDownload(
        Guid fieldId)
    {
        try
        {
            var presignedGetObjectArgs = new PresignedGetObjectArgs()
                .WithBucket("photos")
                .WithObject(fieldId.ToString())
                .WithExpiry(60 * 60 * 24);
            
            var url = await _minioClient.PresignedGetObjectAsync(presignedGetObjectArgs);
            Console.WriteLine(url);
            return Result.Success<string, Error>(fieldId.ToString());
            
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Fail to upload file in minio");
            return Error.Failure("file.upload", "Fail to upload file in minio");
        }
    }
}