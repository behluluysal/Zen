using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text;
using Zen.API.Handlers;
using Zen.Application.Common.Interfaces;
using Zen.Infrastructure.BackgroundJobs;
using Zen.Infrastructure.Data;


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
    public static IHostApplicationBuilder AddZenApiDefaults<TDbContext>(this IHostApplicationBuilder builder,
        Action<ApiBuilderOptions> configureOptions) where TDbContext : DbContext, IZenDbContext
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

        builder.Services.AddControllers(mvcOptions => mvcOptions
            .AddResultConvention(resultStatusMap => resultStatusMap
                .AddDefaultMap()
                .For(ResultStatus.Unauthorized, HttpStatusCode.Unauthorized, resultStatusOptions => resultStatusOptions
                    .With(typeof(object), (ctrlr, result) => new
                    {
                        title = "Unauthorized.",
                        detail = FormatErrors(result.Errors),
                        status = 401
                    }))
            )
        );

        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();

        builder.Services.AddScoped<IZenUserContext, ZenUserContext>();

        builder.Services.AddTransient<ZenJobScheduler<TDbContext>>();
        builder.Services.AddHangfire(configuration => configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseInMemoryStorage())
            .AddHangfireServer();

        builder.Services.AddExceptionHandler<ZenExceptionHandler>();

        return builder;
    }

    private static string FormatErrors(IEnumerable<string> errors)
    {
        var stringBuilder = new StringBuilder("Next error(s) occurred:");
        foreach (var error in errors)
        {
            stringBuilder.AppendLine($"* {error}");
        }
        return stringBuilder.ToString();
    }
}
