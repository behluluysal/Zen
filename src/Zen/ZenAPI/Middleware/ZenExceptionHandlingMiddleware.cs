using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;
using Zen.API.Models;
using Zen.Application.Common.Exceptions;

namespace Zen.API.Middleware;

public class ZenExceptionHandlingMiddleware(RequestDelegate next, ILogger<ZenExceptionHandlingMiddleware> logger)
{
    private readonly RequestDelegate _next = next;
    private readonly ILogger<ZenExceptionHandlingMiddleware> _logger = logger;
    private static readonly JsonSerializerOptions options = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred.");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        if (exception is ZenValidationException validationEx)
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            var response = new ZenValidationErrorResponse(
                exception.Message,
                exception.Message,
                validationEx.Errors);
            var json = JsonSerializer.Serialize(response, options);
            await context.Response.WriteAsync(json);
        }
        else
        {
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            var response = new ZenErrorResponse(
                exception.Message,
                exception.Message);
            var json = JsonSerializer.Serialize(response, options);
            await context.Response.WriteAsync(json);
        }
    }
}