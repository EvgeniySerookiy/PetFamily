namespace PetFamily.Infrastructure.Options;

public class SoftDeleteOptions
{
    public TimeSpan CheckPeriod { get; init; } = TimeSpan.FromDays(24);
    public TimeSpan TimeToRestore { get; init; } = TimeSpan.FromDays(30);
}