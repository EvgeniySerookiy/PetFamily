using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PetFamily.Infrastructure.Options;

namespace PetFamily.Infrastructure.BackgroundServices;

public class SoftDeleteCleanupService : Microsoft.Extensions.Hosting.BackgroundService
{
    private readonly ApplicationDbContex _context;
    private readonly ILogger<SoftDeleteCleanupService> _logger;
    private readonly SoftDeleteOptions _softDeleteOptions;

    public SoftDeleteCleanupService(
        IDbContextFactory<ApplicationDbContex> dbContextFactory, 
        IOptions<SoftDeleteOptions> options, 
        ILogger<SoftDeleteCleanupService> logger)
    {
        _context = dbContextFactory.CreateDbContext();
        _softDeleteOptions = options.Value;
        _logger = logger;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("SoftDeleteCleanupService is starting.");
        
        using PeriodicTimer timer = new(_softDeleteOptions.CheckPeriod);

        while (stoppingToken.IsCancellationRequested == false &&
               await timer.WaitForNextTickAsync(stoppingToken))
        try
        {
            var deletedVolunteers = _context.Volunteers
                .Where(v => v.IsDeleted &&
                            DateTime.UtcNow - v.DeletionDate >= _softDeleteOptions.TimeToRestore);
            
            _context.Volunteers.RemoveRange(deletedVolunteers);
            
            await _context.SaveChangesAsync(stoppingToken);
            
            _logger.LogInformation("Soft delete cleanup completed successfully");
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, exception.Message);
        }
    }
}
