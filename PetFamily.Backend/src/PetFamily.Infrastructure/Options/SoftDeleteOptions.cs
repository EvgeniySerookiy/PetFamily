namespace PetFamily.Infrastructure.Options;

public class SoftDeleteOptions
{
    public TimeSpan CheckPeriod { get; init; } = TimeSpan.FromSeconds(10);
    public TimeSpan TimeToRestore { get; init; } = TimeSpan.FromSeconds(40);
}