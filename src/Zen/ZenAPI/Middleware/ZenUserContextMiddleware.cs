using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using Zen.Application.Common.Interfaces;

namespace Zen.API.Middleware;

public class ZenUserContextMiddleware(RequestDelegate next, ILogger<ZenUserContextMiddleware> logger)
{
    private readonly RequestDelegate _next = next;
    private readonly ILogger<ZenUserContextMiddleware> _logger = logger;

    public async Task InvokeAsync(HttpContext context, IZenUserContext zenUserContext)
    {
        // Extract the user ID from the JWT token
        var userId = context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!string.IsNullOrEmpty(userId))
        {
            zenUserContext.UserId = userId;
            _logger.LogDebug("ZenUserContext set with UserId: {UserId}", userId);
        }
        else
        {
            _logger.LogDebug("No user identity found in the request.");
        }

        await _next(context);
    }
}
