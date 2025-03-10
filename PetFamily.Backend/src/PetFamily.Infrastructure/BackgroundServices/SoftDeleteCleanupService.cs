using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace PetFamily.Infrastructure.BackgroundServices;

public class SoftDeleteCleanupService : Microsoft.Extensions.Hosting.BackgroundService
{
    private readonly ApplicationDbContex _context;
    private readonly ILogger<SoftDeleteCleanupService> _logger;
    private readonly TimeSpan _checkPeriod;
    private readonly TimeSpan _timeToRestore;

    public SoftDeleteCleanupService(
        IDbContextFactory<ApplicationDbContex> dbContextFactory, 
        IOptions<SoftDeleteOptions> options, 
        ILogger<SoftDeleteCleanupService> logger)
    {
        _context = dbContextFactory.CreateDbContext();
        _checkPeriod = options.Value.CheckPeriod;
        _timeToRestore = options.Value.TimeToRestore;
        _logger = logger;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("SoftDeleteCleanupService is starting.");
        
        using PeriodicTimer timer = new(_checkPeriod);

        while (stoppingToken.IsCancellationRequested == false &&
               await timer.WaitForNextTickAsync(stoppingToken))
        try
        {
            var deletedVolunteers = _context.Volunteers
                .Where(v => v.IsDeleted &&
                            DateTime.UtcNow - v.DeletionDate >= _timeToRestore);
            
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
