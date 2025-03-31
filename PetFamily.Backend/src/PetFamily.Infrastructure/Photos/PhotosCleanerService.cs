using PetFamily.Application.Messaging;
using PetFamily.Application.Photos;
using PetFamily.Application.Providers;

namespace PetFamily.Infrastructure.Photos;

public class PhotosCleanerService : IPhotosCleanerService
{
    private readonly IFileProvider _fileProvider;
    private readonly IMessageQueue<IEnumerable<PhotoInfo>> _messageQueue;

    public PhotosCleanerService(
        IFileProvider fileProvider,
        IMessageQueue<IEnumerable<PhotoInfo>> messageQueue)
    {
        _fileProvider = fileProvider;
        _messageQueue = messageQueue;
    }

    public async Task Process(CancellationToken cancellationToken)
    {
        var photoInfos = await _messageQueue.ReadAsync(cancellationToken);

        foreach (var photoInfo in photoInfos)
        {
            await _fileProvider.DeletePhoto(photoInfo, cancellationToken);
        }
    }
}