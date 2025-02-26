﻿namespace Zen.Domain.Utilities;


/// <summary>
/// Represents a domain exception with an associated error code.
/// This interface can be implemented by custom exceptions thrown from domain logic.
/// </summary>
public interface IDomainException
{
    int ErrorCode { get; }
    string Message { get; }
}

/// <summary>
/// Base domain exception with an error code.
/// </summary>
public abstract class DomainException(string message, int errorCode) : Exception(message), IDomainException
{
    public int ErrorCode { get; } = errorCode;
}