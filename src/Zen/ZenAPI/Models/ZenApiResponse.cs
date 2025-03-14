namespace Zen.API.Models;

public class ApiError(int code, string message)
{
    public int Code { get; set; } = code;
    public string Message { get; set; } = message;
}

/// <summary>
/// Generic response envelope for API operations.
/// </summary>
public class ZenApiResponse<T>
{
    public bool IsSuccess { get; set; }
    public T? Data { get; set; }
    public ApiError? Error { get; set; }

    public static ZenApiResponse<T> Success(T data) =>
        new()
        { IsSuccess = true, Data = data };

    public static ZenApiResponse<T> Failure(int errorCode, string errorMessage) =>
        new()
        { IsSuccess = false, Error = new ApiError(errorCode, errorMessage) };
}

/// <summary>
/// Non-generic response envelope.
/// </summary>
public class ZenApiResponse : ZenApiResponse<object>
{
    public static ZenApiResponse Success() =>
        new()
        { IsSuccess = true };

    public new static ZenApiResponse Failure(int errorCode, string errorMessage) =>
        new()
        { IsSuccess = false, Error = new ApiError(errorCode, errorMessage) };
}