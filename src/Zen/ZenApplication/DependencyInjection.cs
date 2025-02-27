using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Zen.Application.MediatR.Behaviors;

namespace Zen.Application.DependencyInjection
{
    public class ApplicationSetupOptions
    {
#nullable disable
        [Required]
        public string Assembly { get; set; }
        [Required]
        public IConfiguration Configuration { get; set; }
        public bool UseAutoMapper { get; set; } = true;
        public bool UseFluentValidation { get; set; } = true;
        public bool UseMediatR { get; set; } = true;
#nullable enable
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
            var options = new ApplicationSetupOptions();
            configureOptions(options);

            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(options);
            if (!Validator.TryValidateObject(options, validationContext, validationResults, true))
            {
                var errors = string.Join(", ", validationResults.Select(vr => vr.ErrorMessage));
                throw new InvalidOperationException($"Invalid options: {errors}");
            }

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
    }
}
