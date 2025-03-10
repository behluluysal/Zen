using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;
using Zen.Application.MediatR.Behaviors;

namespace Zen.Application.DependencyInjection;

public class ApplicationSetupOptions
{
    public string Assembly { get; set; } = string.Empty;
    public IConfiguration Configuration { get; set; } = default!;
    public bool UseAutoMapper { get; set; } = true;
    public bool UseFluentValidation { get; set; } = true;
    public bool UseMediatR { get; set; } = true;
}

public static class ApplicationServiceBuilder
{
    /// <summary>
    /// Configures and registers application-level services such as AutoMapper, FluentValidation, 
    /// and MediatR based on the provided setup options. Validates the configuration before applying.
    /// </summary>
    /// <param name="builder">The application host builder.</param>
    /// <param name="configureOptions">A delegate to configure <see cref="ApplicationSetupOptions"/>.</param>
    /// <returns>The modified <see cref="IHostApplicationBuilder"/> instance.</returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown if the provided options are invalid or missing required values.
    /// </exception>

    public static IHostApplicationBuilder AddZenApplication(this IHostApplicationBuilder builder,
        Action<ApplicationSetupOptions> configureOptions)
    {
        ApplicationSetupOptions options = Validate(configureOptions);

        Assembly applicationAssembly = Assembly.Load(options.Assembly);

        if (options.UseAutoMapper)
        {
            builder.Services.AddAutoMapper(applicationAssembly);
        }

        if (options.UseFluentValidation)
        {
            builder.Services.AddValidatorsFromAssembly(applicationAssembly);
        }

        if (options.UseMediatR)
        {
            builder.Services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(applicationAssembly);
            });
            builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ZenCommandActionBehavior<,>));
        }

        return builder;
    }

    private static ApplicationSetupOptions Validate(Action<ApplicationSetupOptions> configureOptions)
    {
        ApplicationSetupOptions options = new();
        configureOptions(options);

        var validator = new ApplicationSetupOptionsValidator();
        var validationResult = validator.Validate(options);
        if (!validationResult.IsValid)
        {
            var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
            throw new InvalidOperationException($"Invalid options: {errors}");
        }
        return options;
    }

    private sealed class ApplicationSetupOptionsValidator : AbstractValidator<ApplicationSetupOptions>
    {
        public ApplicationSetupOptionsValidator()
        {
            RuleFor(x => x.Assembly)
                .NotEmpty().WithMessage("Assembly is required.");
            RuleFor(x => x.Configuration)
                .NotNull().WithMessage("Configuration must be provided.");
        }
    }
}
