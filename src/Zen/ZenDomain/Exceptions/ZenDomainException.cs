namespace Zen.Domain.Exceptions;


/// <summary>
/// Represents a domain exception with an associated error code.
/// This interface can be implemented by custom exceptions thrown from domain logic.
/// </summary>
public interface IZenDomainException
{
    int ErrorCode { get; }
    string Message { get; }
}

/// <summary>
/// Base domain exception with an error code.
/// </summary>
public abstract class ZenDomainException(string message, int errorCode) : Exception(message), IZenDomainException
{
    public int ErrorCode { get; } = errorCode;
}