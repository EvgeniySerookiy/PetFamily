using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using Minio;
using Minio.DataModel.Args;
using PetFamily.Application.Photos;
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

    public async Task<Result<IReadOnlyList<PhotoPath>, Error>> UploadPhotos(
        IEnumerable<PhotoData> photosData,
        CancellationToken cancellationToken = default)
    {
        var semaphoreSlim = new SemaphoreSlim(MAX_DEGREE_OF_PARALLELISM);
        var photosList = photosData.ToList();

        try
        {
            await IfBucketsNotExistCreateBucket(photosList.Select(photo => photo.PhotoInfo.BucketName), cancellationToken);

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

    public async Task<UnitResult<Error>> DeletePhotos(
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

    public async Task<UnitResult<Error>> DeletePhoto(
        PhotoInfo photoInfo,
        CancellationToken cancellationToken = default)
    {
        try
        {
            await IfBucketsNotExistCreateBucket([photoInfo.BucketName], cancellationToken);

            var statArgs = new StatObjectArgs()
                .WithBucket(photoInfo.BucketName)
                .WithObject(photoInfo.PhotoPath.Path);

            var objectStat = await _minioClient.StatObjectAsync(statArgs, cancellationToken);
            if (objectStat is null)
                return Result.Success<Error>();

            var removeArgs = new RemoveObjectsArgs()
                .WithBucket(photoInfo.BucketName)
                .WithObject(photoInfo.PhotoPath.Path);

            await _minioClient.RemoveObjectsAsync(removeArgs, cancellationToken);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Fail to delete file in minio with {path} in bucket {bucket}",
                photoInfo.PhotoPath.Path,
                photoInfo.BucketName);

            return Error.Failure("file.delete", "Fail to delete file in minio");
        }

        return Result.Success<Error>();
    }


    private async Task<Result<PhotoPath, Error>> PutObject(
        PhotoData photoData,
        SemaphoreSlim semaphoreSlim,
        CancellationToken cancellationToken)
    {
        await semaphoreSlim.WaitAsync(cancellationToken);

        var putObjectArgs = new PutObjectArgs()
            .WithBucket(photoData.PhotoInfo.BucketName)
            .WithStreamData(photoData.Stream)
            .WithObjectSize(photoData.Stream.Length)
            .WithObject(photoData.PhotoInfo.PhotoPath.Path);

        try
        {
            await _minioClient
                .PutObjectAsync(putObjectArgs, cancellationToken);

            return photoData.PhotoInfo.PhotoPath;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Fail to upload file in minio with path {path} in bucket {bucket}",
                photoData.PhotoInfo.PhotoPath.Path,
                photoData.PhotoInfo.BucketName);

            return Error.Failure("file.upload", "Fail to upload file in minio");
        }
        finally
        {
            semaphoreSlim.Release();
        }
    }

    private async Task IfBucketsNotExistCreateBucket(
        IEnumerable<string> buckets,
        CancellationToken cancellationToken)
    {
        HashSet<string> bucketNames = [..buckets];

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