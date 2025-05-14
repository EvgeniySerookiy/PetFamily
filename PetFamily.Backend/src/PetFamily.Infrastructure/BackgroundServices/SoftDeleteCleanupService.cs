using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PetFamily.Application.Messaging;
using PetFamily.Application.Photos;
using PetFamily.Domain.PetManagement.Entities;
using PetFamily.Infrastructure.DbContexts;
using PetFamily.Infrastructure.Options;

namespace PetFamily.Infrastructure.BackgroundServices;

public class SoftDeleteCleanupService : BackgroundService
{
    private const string BUCKET_NAME = "photos";

    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ILogger<SoftDeleteCleanupService> _logger;
    private readonly IMessageQueue<IEnumerable<PhotoInfo>> _messageQueue;
    private readonly SoftDeleteOptions _softDeleteOptions;

    public SoftDeleteCleanupService(
        IServiceScopeFactory serviceScopeFactory,
        IOptions<SoftDeleteOptions> options,
        ILogger<SoftDeleteCleanupService> logger,
        IMessageQueue<IEnumerable<PhotoInfo>> messageQueue)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _softDeleteOptions = options.Value;
        _logger = logger;
        _messageQueue = messageQueue;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("SoftDeleteCleanupService is starting.");

        using PeriodicTimer timer = new(_softDeleteOptions.CheckPeriod);

        while (stoppingToken.IsCancellationRequested == false &&
               await timer.WaitForNextTickAsync(stoppingToken))
            try
            {
                await CleanupDeletedPetsAsync(stoppingToken);
                await CleanupDeletedVolunteersAsync(stoppingToken);
                
                _logger.LogInformation("Soft delete cleanup completed successfully");
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, exception.Message);
            }
    }

    private async Task CleanupDeletedVolunteersAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var writeDbContext = scope.ServiceProvider.GetRequiredService<WriteDbContext>();

        var deletedVolunteersCount = writeDbContext.Volunteers
            .Where(v => v.IsDeleted &&
                        DateTime.UtcNow - v.DeletionDate >= _softDeleteOptions.TimeToRestore)
            .ExecuteDeleteAsync(cancellationToken);

        await writeDbContext.SaveChangesAsync(cancellationToken);
    }

    private async Task CleanupDeletedPetsAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var writeDbContext = scope.ServiceProvider.GetRequiredService<WriteDbContext>();

        var deletedPets = await GetDeletedPetsAsync(writeDbContext, cancellationToken);
    
        var photosToDelete = ExtractPhotosToDelete(deletedPets);

        if (photosToDelete.Any())
            await _messageQueue.WriteAsync(photosToDelete, cancellationToken);

        await DeletePetsAsync(writeDbContext, cancellationToken);
    }

    private async Task<List<Pet>> GetDeletedPetsAsync(WriteDbContext context, CancellationToken cancellationToken)
    {
        return await context.Volunteers
            .Include(v => v.Pets)
            .SelectMany(v => v.Pets)
            .Where(p => p.IsDeleted &&
                        DateTime.UtcNow - p.DeletionDate >= _softDeleteOptions.TimeToRestore)
            .ToListAsync(cancellationToken);
    }

    private List<PhotoInfo> ExtractPhotosToDelete(IEnumerable<Pet> deletedPets)
    {
        var photosToDelete = new List<PhotoInfo>();

        foreach (var pet in deletedPets)
        {
            if (pet.PetPhotos == null) continue;

            foreach (var photo in pet.PetPhotos)
                photosToDelete.Add(new PhotoInfo(photo.PathToStorage, BUCKET_NAME));
        }

        return photosToDelete;
    }

    private async Task DeletePetsAsync(WriteDbContext context, CancellationToken cancellationToken)
    {
        await context.Volunteers
            .Include(v => v.Pets)
            .SelectMany(v => v.Pets)
            .Where(p => p.IsDeleted &&
                        DateTime.UtcNow - p.DeletionDate >= _softDeleteOptions.TimeToRestore)
            .ExecuteDeleteAsync(cancellationToken);
    }

}