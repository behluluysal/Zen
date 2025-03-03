using System.Diagnostics;

namespace Zen.AppHost;

internal static class ResourceBuilderExtensions
{
    internal static IResourceBuilder<T> WithSwaggerUI<T>(this IResourceBuilder<T> resourceBuilder) where T : IResourceWithEndpoints
    {
        return resourceBuilder.WithOpenApiDocs("swagger-ui-docs", "Swagger Api Documentation", "swagger");
    }

    private static IResourceBuilder<T> WithOpenApiDocs<T>(this IResourceBuilder<T> resourceBuilder,
        string name,
        string displayName,
        string openApiUiPath) where T : IResourceWithEndpoints
    {
        return resourceBuilder.WithCommand(
            name,
            displayName,
            executeCommand: _ =>
            {
                try
                {
                    var endpoint = resourceBuilder.GetEndpoint("https");
                    var url = $"{endpoint.Url}/{openApiUiPath}";
                    Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
                    return Task.FromResult(new ExecuteCommandResult { Success = true });
                }
                catch (Exception ex)
                {

                    return Task.FromResult(new ExecuteCommandResult { Success = false, ErrorMessage = ex.ToString() });
                }
            },
            updateState: context => context.ResourceSnapshot.HealthStatus == Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Healthy ?
                ResourceCommandState.Enabled : ResourceCommandState.Disabled,
            iconName: "Document",
            iconVariant: IconVariant.Filled);
    }

    internal static IResourceBuilder<T> WithHangfireDashboard<T>(
        this IResourceBuilder<T> resourceBuilder,
        string name = "jobs",
        string displayName = "Hangfire Dashboard",
        string hangfirePath = "jobs")
        where T : IResourceWithEndpoints
    {
        return resourceBuilder.WithCommand(
            name,
            displayName,
            executeCommand: _ =>
            {
                try
                {
                    var endpoint = resourceBuilder.GetEndpoint("https");
                    var url = $"{endpoint.Url}/{hangfirePath}";
                    Process.Start(new ProcessStartInfo(url)
                    {
                        UseShellExecute = true
                    });
                    return Task.FromResult(new ExecuteCommandResult { Success = true });
                }
                catch (Exception ex)
                {
                    return Task.FromResult(new ExecuteCommandResult
                    {
                        Success = false,
                        ErrorMessage = ex.ToString()
                    });
                }
            },
            updateState: context => context.ResourceSnapshot.HealthStatus
                == Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Healthy
                ? ResourceCommandState.Enabled
                : ResourceCommandState.Disabled,
            iconName: "Fire",
            iconVariant: IconVariant.Filled);
    }
}
