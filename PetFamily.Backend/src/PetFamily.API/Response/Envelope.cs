using PetFamily.Domain.Shared.ErrorContext;

namespace PetFamily.API.Response;

public record Envelope
{
    public object? Result { get; }
    public string? ErrorCode { get; }
    public string? ErrorMessage { get; }
    public DateTime TimeGenarated { get; }

    private Envelope(object? result, Error? error)
    {
        Result = result;
        ErrorCode = error?.Code;
        ErrorMessage = error?.Message;
        TimeGenarated = DateTime.Now;
    }
    
    public static Envelope Ok(object? result) =>
        new (result, null);
    
    public static Envelope Error(Error error) =>
        new (null, error);
}