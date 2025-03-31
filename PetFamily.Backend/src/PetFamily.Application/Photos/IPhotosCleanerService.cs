namespace PetFamily.Application.Photos;

public interface IPhotosCleanerService
{
    Task Process(CancellationToken cancellationToken);
}