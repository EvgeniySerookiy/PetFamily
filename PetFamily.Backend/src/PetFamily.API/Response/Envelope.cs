using PetFamily.Domain.Shared.ErrorContext;

namespace PetFamily.API.Response;

public record ResponceError(
    string? ErrorCode,
    string? ErrorMessage,
    string? InvalidField);

public record Envelope
{
    public object? Result { get; }
    List<ResponceError> Errors { get; } 
    public DateTime TimeGenarated { get; }

    private Envelope(object? result, IEnumerable<ResponceError> errors)
    {
        Result = result;
        Errors = errors.ToList() ;
        TimeGenarated = DateTime.Now;
    }
    
    public static Envelope Ok(object? result) =>
        new (result, []);
    
    public static Envelope Error(IEnumerable<ResponceError> errors) =>
        new (null, errors);
}