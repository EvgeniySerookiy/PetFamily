using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using Minio;
using Minio.DataModel.Args;
using PetFamily.Application.FileProvider;
using PetFamily.Application.Providers;
using PetFamily.Domain.Shared.ErrorContext;

namespace PetFamily.Infrastructure.Providers;

public class MinioProvider : IFileProvider
{
    private const int MAX_DEGREE_OF_PARALLELISM = 5;

    private readonly IMinioClient _minioClient;
    private readonly ILogger<MinioProvider> _logger;

    public MinioProvider(IMinioClient minioClient, ILogger<MinioProvider> logger)
    {
        _minioClient = minioClient;
        _logger = logger;
    }

    public async Task<UnitResult<Error>> UploadFiles(
        FileData fileData,
        CancellationToken cancellationToken = default)
    {
        var semaphoreSlim = new SemaphoreSlim(MAX_DEGREE_OF_PARALLELISM);

        try
        {
            var buketExistArgs = new BucketExistsArgs()
                .WithBucket(fileData.BucketName);

            var bucketExist = await _minioClient.BucketExistsAsync(buketExistArgs, cancellationToken);

            if (bucketExist == false)
            {
                var makeBuketArgs = new MakeBucketArgs()
                    .WithBucket(fileData.BucketName);

                await _minioClient.MakeBucketAsync(makeBuketArgs, cancellationToken);
            }

            List<Task> tasks = [];

            foreach (var file in fileData.Files)
            {
                await semaphoreSlim.WaitAsync(cancellationToken);
                var putObjectsArgs = new PutObjectArgs()
                    .WithBucket(fileData.BucketName)
                    .WithStreamData(file.Stream)
                    .WithObjectSize(file.Stream.Length)
                    .WithObject(file.ObjectName);

                var task = _minioClient.PutObjectAsync(putObjectsArgs, cancellationToken);

                semaphoreSlim.Release();

                tasks.Add(task);
            }

            await Task.WhenAll(tasks);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Fail to upload file in minio");
            return Error.Failure("file.upload", "Fail to upload file in minio");
        }
        finally
        {
            semaphoreSlim.Release();
        }
        
        return Result.Success<Error>();
    }

    public async Task<UnitResult<Error>> DeleteFiles(
        CollectionsObjectName collectionsObjectName,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var removeObjectArgs = new RemoveObjectsArgs()
                .WithBucket(collectionsObjectName.BucketName)
                .WithObjects(collectionsObjectName.ObjectNames
                    .Select(obj => obj.Value)
                    .ToList());

            await _minioClient.RemoveObjectsAsync(removeObjectArgs, cancellationToken);

            return Result.Success<Error>();
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Fail to delete file in minio");
            return Error.Failure("file.delete", "Fail to delete file in minio");
        }
    }

    public async Task<Result<string, Error>> GetFileDownloadUrl(
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
            _logger.LogError(exception, "Failed to generate presigned URL for file in minio");
            return Error.Failure("file.download", "Failed to generate presigned URL for file in minio");
        }
    }
}