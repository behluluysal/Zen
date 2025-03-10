namespace Zen.Application.MediatR.Common;

/// <summary>
/// Standard operation result wrapper used by application services.
/// </summary>
public class ZenOperationResult
{
    public bool IsSuccess { get; set; }
    public int ErrorCode { get; set; }
    public required string ErrorMessage { get; set; }

    public static ZenOperationResult Success() => new()
    {
        IsSuccess = true,
        ErrorMessage = string.Empty
    };

    public static ZenOperationResult Failure(int errorCode, string errorMessage) =>
        new()
        {
            IsSuccess = false,
            ErrorCode = errorCode,
            ErrorMessage = errorMessage
        };
}

/// <summary>
/// Generic operation result wrapper carrying data.
/// </summary>
public class ZenOperationResult<T> : ZenOperationResult
{
    public required T Data { get; set; }

    public static ZenOperationResult<T> Success(T data) =>
        new()
        {
            IsSuccess = true,
            Data = data,
            ErrorMessage = string.Empty
        };

    public static new ZenOperationResult<T> Failure(int errorCode, string errorMessage) =>
        new()
        {
            IsSuccess = false,
            ErrorCode = errorCode,
            ErrorMessage = errorMessage,
            Data = default!
        };
}