namespace PetFamily.Infrastructure.BackgroundServices;

public class SoftDeleteOptions
{
    public TimeSpan CheckPeriod { get; set; } = TimeSpan.FromDays(24);
    public TimeSpan TimeToRestore { get; set; } = TimeSpan.FromDays(30);
}