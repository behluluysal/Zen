namespace Zen.Application.Utilities.Common;

/// <summary>
/// Standard operation result wrapper used by application services.
/// </summary>
public class OperationResult
{
    public bool IsSuccess { get; set; }
    public int ErrorCode { get; set; }
    public required string ErrorMessage { get; set; }

    public static OperationResult Success() => new()
    {
        IsSuccess = true,
        ErrorMessage = string.Empty
    };

    public static OperationResult Failure(int errorCode, string errorMessage) =>
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
public class OperationResult<T> : OperationResult
{
    public required T Data { get; set; }

    public static OperationResult<T> Success(T data) =>
        new()
        {
            IsSuccess = true,
            Data = data,
            ErrorMessage = string.Empty
        };

    public static new OperationResult<T> Failure(int errorCode, string errorMessage) =>
        new()
        {
            IsSuccess = false,
            ErrorCode = errorCode,
            ErrorMessage = errorMessage,
            Data = default!
        };
}