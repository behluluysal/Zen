namespace Zen.API.Models;

public class ApiError(string title, string message)
{
    public string Title { get; set; } = title;
    public string Message { get; set; } = message;
}

/// <summary>
/// Non-generic Response for successful API calls.
/// </summary>
public class ZenSuccessResponse
{
}

/// <summary>
/// Response for successful API calls.
/// </summary>
public class ZenSuccessResponse<T>(T data) : ZenSuccessResponse
{
    public T Data { get; set; } = data;
}

/// <summary>
/// Response for general errors.
/// </summary>
public class ZenErrorResponse(string title, string message)
{
    public ApiError Error { get; set; } = new ApiError(title, message);
}

/// <summary>
/// Response for validation errors.
/// </summary>
public class ZenValidationErrorResponse(string title, string message, IDictionary<string, string[]> validationErrors) : ZenErrorResponse(title, message)
{
    public IDictionary<string, string[]> ValidationErrors { get; set; } = validationErrors;
}
