namespace PetFamily.Application.Messaging;

public interface IMessageQueue<IMessage>
{
    Task WriteAsync(IMessage path, CancellationToken cancellationToken = default);

    Task<IMessage> ReadAsync(CancellationToken cancellationToken = default); 
}