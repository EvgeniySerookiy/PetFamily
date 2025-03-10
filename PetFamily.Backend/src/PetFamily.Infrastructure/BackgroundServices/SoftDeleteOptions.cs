namespace PetFamily.Infrastructure.BackgroundService;

public class SoftDeleteOptions
{
    public TimeSpan CheckPeriod { get; set; }
    public TimeSpan TimeToRestore { get; set; }
}