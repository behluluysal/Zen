using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.ComponentModel.DataAnnotations;
using Zen.Application.Utilities.Transaction;
using Zen.Domain.Repositories;
using Zen.Infrastructure.Data;
using Zen.Infrastructure.Data.Interceptors;
using Zen.Infrastructure.Data.Security;
using Zen.Infrastructure.Repositories;

namespace Zen.Infrastructure.DependencyInjection;

public class InfrastructureSetupOptions
{
#nullable disable
    [Required]
    public string Assembly { get; set; }

    [Required]
    public string AspireDbName { get; set; }

    [Required]
    public string ColumnHashingSecret { get; set; }
#nullable enable
}

public static class InfrastructureServiceBuilder
{
    /// <summary>
    /// Configures and registers infrastructure-level services, including database context, 
    /// unit of work, and other dependencies based on the provided setup options. 
    /// Validates the configuration before applying.
    /// </summary>
    /// <typeparam name="TDbContext">The type of the database context, derived from <see cref="ZenDbContext"/>.</typeparam>
    /// <param name="builder">The application host builder.</param>
    /// <param name="configureOptions">A delegate to configure <see cref="InfrastructureSetupOptions"/>.</param>
    /// <returns>The modified <see cref="IHostApplicationBuilder"/> instance.</returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown if the provided options are invalid or missing required values.
    /// </exception>

    public static IHostApplicationBuilder AddZenInfrastructure<TDbContext>(
        this IHostApplicationBuilder builder,
        Action<InfrastructureSetupOptions> configureOptions) where TDbContext : DbContext, IZenDbContext
    {
        var options = new InfrastructureSetupOptions();
        configureOptions(options);

        var context = new ValidationContext(options);
        var results = new List<ValidationResult>();
        if (!Validator.TryValidateObject(options, context, results, true))
        {
            var errors = string.Join(", ", results.Select(r => r.ErrorMessage));
            throw new InvalidOperationException($"Invalid InfrastructureSetupOptions: {errors}");
        }

        builder.Services.AddSingleton<ConvertDomainEventsToOutboxMessagesInterceptor>();
        
        builder.ConfigureDatabase<TDbContext>(options);

        builder.Services.AddScoped<IUnitOfWork, EfUnitOfWork<TDbContext>>();

        builder.Services.AddScoped<IAuditHistoryRepository, AuditHistoryRepository<TDbContext>>();

        return builder;
    }

    private static void ConfigureDatabase<TDbContext>(this IHostApplicationBuilder builder, InfrastructureSetupOptions options) where TDbContext : DbContext, IZenDbContext
    {
        builder.Services.AddSingleton<IColumnEncryptionService>(sp =>
               new AesColumnEncryptionService(options.ColumnHashingSecret));

        var encryptionService = builder.Services.BuildServiceProvider().GetRequiredService<IColumnEncryptionService>();
        ZenDbContext.StaticColumnEncryptionService = encryptionService;

        var outboxMessageInterceptor = builder.Services.BuildServiceProvider()
                       .GetService<ConvertDomainEventsToOutboxMessagesInterceptor>()
                       ?? throw new ArgumentException("ConvertDomainEventsToOutboxMessagesInterceptor was null!");

        builder.AddSqlServerDbContext<TDbContext>(
            options.AspireDbName,
            configureDbContextOptions: opt =>
            {
                opt.UseSqlServer(sqlOptions =>
                {
                    sqlOptions.MigrationsAssembly(options.Assembly);
                    opt.AddInterceptors(outboxMessageInterceptor);
                });
            }
        );
        
        builder.Services.AddDbContextFactory<TDbContext>(opt =>
        {
            opt.UseSqlServer(sqlOptions =>
            {
                sqlOptions.MigrationsAssembly(options.Assembly);
            });
            opt.AddInterceptors(outboxMessageInterceptor);
        });
    }
}
