using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Photos;

namespace PetFamily.Infrastructure.BackgroundServices;

public class PhotosCleanupBackgroundService : BackgroundService
{
    private readonly ILogger<PhotosCleanupBackgroundService> _logger;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public PhotosCleanupBackgroundService(
        ILogger<PhotosCleanupBackgroundService> logger,
        IServiceScopeFactory serviceScopeFactory)
    {
        _logger = logger;
        _serviceScopeFactory = serviceScopeFactory;
    }
    
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("PhotosCleanupBackgroundService is running.");
        
        await using var scope = _serviceScopeFactory.CreateAsyncScope();
            
        var photosCleanerService = scope.ServiceProvider.GetRequiredService<IPhotosCleanerService>();
        
        while (stoppingToken.IsCancellationRequested == false)
        {
            await photosCleanerService.Process(stoppingToken);

            await Task.CompletedTask;
        }
    }
}