namespace Zen.API.Models;

/// <summary>
/// Base class for all API responses, ensuring a consistent structure.
/// </summary>
public abstract class ZenApiResponseBase(bool isSuccess, ApiError? error)
{
    public bool IsSuccess { get; set; } = isSuccess;
    public ApiError? Error { get; set; } = error;
}

public class ZenApiResponse : ZenApiResponseBase
{
    private ZenApiResponse(bool isSuccess, ApiError? error)
        : base(isSuccess, error) { }

    public static ZenApiResponse Success() => new(true, null);

    public static ZenApiResponse Failure(int errorCode, string errorMessage) =>
        new(false, new ApiError(errorCode, errorMessage));
}

public class ZenApiResponse<T> : ZenApiResponseBase
{
    public T Data { get; set; }

    public ZenApiResponse(T data)
        : base(true, null) => Data = data;

    public ZenApiResponse(ApiError error)
        : base(false, error) => Data = default!;
    
    public static ZenApiResponse<T> Success(T data) => new(data);

    public static ZenApiResponse<T> Failure(int errorCode, string errorMessage) => 
        new(new ApiError(errorCode, errorMessage));
}

public class ApiError(int code, string message)
{
    public int Code { get; set; } = code;
    public string Message { get; set; } = message;
}
