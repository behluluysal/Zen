using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;


namespace Zen.API.Extensions;

public class ApiBuilderOptions
{
#nullable disable
    public Action<IConfiguration> ConfigureLayers { get; set; }

#nullable enable
}

public static class ZenApiBuilderExtensions
{
    /// <summary>
    /// Configures default API services and logging settings, including OpenTelemetry 
    /// and other service defaults. This method should be called during host building.
    /// </summary>
    /// <param name="builder">The application host builder.</param>
    /// <param name="configureOptions">A delegate to configure <see cref="ApiBuilderOptions"/>.</param>
    /// <returns>The modified <see cref="IHostApplicationBuilder"/> instance.</returns>
    public static IHostApplicationBuilder AddZenApiDefaults(this IHostApplicationBuilder builder,
        Action<ApiBuilderOptions> configureOptions)
    {
        ApiBuilderOptions options = new();
        configureOptions(options);

        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(options);
        if (!Validator.TryValidateObject(options, validationContext, validationResults, true))
        {
            var errors = string.Join(", ", validationResults.Select(vr => vr.ErrorMessage));
            throw new InvalidOperationException($"Invalid options: {errors}");
        }

        builder.Logging.AddOpenTelemetry(logging =>
        {
            logging.IncludeFormattedMessage = true;
            logging.IncludeScopes = true;
            logging.ParseStateValues = true;
        });

        var configuration = builder.Configuration;
        options.ConfigureLayers?.Invoke(configuration);

        builder.Services.AddControllers();
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();

        return builder;
    }
}
