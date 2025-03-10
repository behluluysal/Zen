using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Zen.Application.MediatR.Behaviors;

/// <summary>
/// Pipeline behavior that logs request processing and measures execution time.
/// </summary>
public class ZenCommandActionBehavior<TRequest, TResponse>(ILogger<ZenCommandActionBehavior<TRequest, TResponse>> logger) : IPipelineBehavior<TRequest, TResponse>
     where TRequest : notnull
{
    private readonly ILogger<ZenCommandActionBehavior<TRequest, TResponse>> _logger = logger;

    private const string HandlingTemplate = "Handling {RequestName}";
    private const string HandledTemplate = "Handled {RequestName} in {ElapsedMilliseconds}ms";
    private const string ErrorTemplate = "Error handling {RequestName} after {ElapsedMilliseconds}ms";

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        string requestName = typeof(TRequest).Name;
        _logger.LogInformation(HandlingTemplate, requestName);

        var stopwatch = Stopwatch.StartNew();

        try
        {
            TResponse response = await next();
            stopwatch.Stop();

            _logger.LogInformation(HandledTemplate, requestName, stopwatch.ElapsedMilliseconds);
            return response;
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            _logger.LogError(ex, ErrorTemplate, requestName, stopwatch.ElapsedMilliseconds);
            throw;
        }
    }
}
