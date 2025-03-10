namespace Zen.Application.Exceptions;

/// <summary>
/// Represents a domain exception with an associated error code.
/// This interface can be implemented by custom exceptions thrown from application logic.
/// </summary>
public interface IZebApplicationException
{
    int ErrorCode { get; }
    string Message { get; }
}

/// <summary>
/// Base application exception with an error code.
/// </summary>
public abstract class ZenApplicationException(string message, int errorCode) : Exception(message), IZebApplicationException
{
    public int ErrorCode { get; } = errorCode;
}