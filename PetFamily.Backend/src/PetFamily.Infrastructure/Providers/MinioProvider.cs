using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using Minio;
using Minio.DataModel.Args;
using PetFamily.Application.FileProvider;
using PetFamily.Application.Providers;
using PetFamily.Domain.PetManagement.PetVO;
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

    public async Task<Result<IReadOnlyList<PhotoPath>, Error>> UploadFiles(
        IEnumerable<PhotoData> photosData,
        CancellationToken cancellationToken = default)
    {
        var semaphoreSlim = new SemaphoreSlim(MAX_DEGREE_OF_PARALLELISM);
        var photosList = photosData.ToList();

        try
        {
            await IfBucketsNotExistCreateBucket(photosList, cancellationToken);

            var tasks = photosList.Select(async photo =>
                await PutObject(photo, semaphoreSlim, cancellationToken));

            var pathsResult = await Task.WhenAll(tasks);

            if (pathsResult.Any(p => p.IsFailure))
                return pathsResult.First().Error;

            var results = pathsResult.Select(p => p.Value).ToList();

            return results;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Fail to upload files in minio, files amount: {amount}", photosList.Count);

            return Error.Failure("file.upload", "Fail to upload files in minio");
        }
    }

    public async Task<UnitResult<Error>> DeleteFiles(
        PhotosPathWithBucket photosPathWithBucket,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var removeObjectArgs = new RemoveObjectsArgs()
                .WithBucket(photosPathWithBucket.BucketName)
                .WithObjects(photosPathWithBucket.PhotosPath
                    .Select(obj => obj.Path)
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
    

    private async Task<Result<PhotoPath, Error>> PutObject(
        PhotoData photoData,
        SemaphoreSlim semaphoreSlim,
        CancellationToken cancellationToken)
    {
        await semaphoreSlim.WaitAsync(cancellationToken);

        var putObjectArgs = new PutObjectArgs()
            .WithBucket(photoData.BucketName)
            .WithStreamData(photoData.Stream)
            .WithObjectSize(photoData.Stream.Length)
            .WithObject(photoData.PhotoPath.Path);

        try
        {
            await _minioClient
                .PutObjectAsync(putObjectArgs, cancellationToken);

            return photoData.PhotoPath;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Fail to upload file in minio with path {path} in bucket {bucket}",
                photoData.PhotoPath.Path,
                photoData.BucketName);

            return Error.Failure("file.upload", "Fail to upload file in minio");
        }
        finally
        {
            semaphoreSlim.Release();
        }
    }

    private async Task IfBucketsNotExistCreateBucket(
        IEnumerable<PhotoData> photosData,
        CancellationToken cancellationToken)
    {
        HashSet<string> bucketNames = [..photosData.Select(file => file.BucketName)];

        foreach (var bucketName in bucketNames)
        {
            var bucketExistArgs = new BucketExistsArgs()
                .WithBucket(bucketName);

            var bucketExist = await _minioClient
                .BucketExistsAsync(bucketExistArgs, cancellationToken);

            if (bucketExist == false)
            {
                var makeBucketArgs = new MakeBucketArgs()
                    .WithBucket(bucketName);

                await _minioClient.MakeBucketAsync(makeBucketArgs, cancellationToken);
            }
        }
    }
}