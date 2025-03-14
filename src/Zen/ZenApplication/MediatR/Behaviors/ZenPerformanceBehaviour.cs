using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using Zen.Application.Common.Interfaces;

namespace Zen.Application.MediatR.Behaviors;

/// <summary>
/// Pipeline behavior that measures execution time.
/// </summary>
public class ZenPerformanceBehaviour<TRequest, TResponse>(ILogger<ZenPerformanceBehaviour<TRequest, TResponse>> logger, IZenUserContext userContext) 
    : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        string requestName = typeof(TRequest).Name;

        var stopwatch = Stopwatch.StartNew();
        TResponse response = await next();
        stopwatch.Stop();

        var elapsedMilliseconds = stopwatch.ElapsedMilliseconds;

        if (elapsedMilliseconds > 500)
        {
            var userId = userContext.UserId ?? string.Empty;

            logger.LogWarning("Long Running Request: {Name} ({ElapsedMilliseconds} milliseconds) {@UserId} {@Request}",
                requestName, elapsedMilliseconds, userId, request);
        }
        return response;
    }
}
