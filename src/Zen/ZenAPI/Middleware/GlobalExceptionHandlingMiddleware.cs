using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace Zen.API.Middleware;

public class GlobalExceptionHandlingMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlingMiddleware> logger)
{
    private readonly RequestDelegate _next = next;
    private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger = logger;

    public async Task Invoke(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(httpContext, ex, _logger);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception, ILogger logger)
    {
        // Log exception details (you might want to log stack traces, inner exceptions, etc.)
        logger.LogError(exception, "An unhandled exception occurred.");

        // Map exceptions to appropriate status codes.
        var statusCode = (int)HttpStatusCode.InternalServerError;
        var errorMessage = "An unexpected error occurred.";

        // Optionally, you can inspect exception type and provide custom messages.
        if (exception is ValidationException)
        {
            statusCode = (int)HttpStatusCode.BadRequest;
            errorMessage = exception.Message;
        }
        else if (exception is ApplicationException)
        {
            statusCode = (int)HttpStatusCode.BadRequest;
            errorMessage = exception.Message;
        }

        var response = new ApiResponse<object>
        {
            Success = false,
            ErrorCode = statusCode,
            ErrorMessage = errorMessage,
            Data = null
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;
        var json = JsonConvert.SerializeObject(response);
        return context.Response.WriteAsync(json);
    }
}

/// <summary>
/// A standard API response wrapper.
/// </summary>
public class ApiResponse<T>
{
    public bool Success { get; set; }
    public int ErrorCode { get; set; }
    public string ErrorMessage { get; set; }
    public T Data { get; set; }
}