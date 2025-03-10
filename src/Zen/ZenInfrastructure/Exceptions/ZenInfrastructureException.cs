namespace Zen.Infrastructure.Exceptions;

/// <summary>
/// Represents a infrastructure exception with an associated error code.
/// This interface can be implemented by custom exceptions thrown from infrastructure logic.
/// </summary>
public interface IZenInfrastructureException
{
    int ErrorCode { get; }
    string Message { get; }
}

/// <summary>
/// Base infrastructure exception with an error code.
/// </summary>
public abstract class ZenInfrastructureException(string message, int errorCode) : Exception(message), IZenInfrastructureException
{
    public int ErrorCode { get; } = errorCode;
}