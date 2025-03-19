using Ardalis.GuardClauses;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Zen.Application.Common.Interfaces;
using Zen.Domain.Aggregates;
using Zen.Infrastructure.Data;
using Zen.Infrastructure.Data.Interceptors;
using Zen.Infrastructure.Data.Security;
using Zen.Infrastructure.Services;

namespace Zen.Infrastructure.DependencyInjection;

public class InfrastructureSetupOptions
{
    public string Assembly { get; set; } = string.Empty;
    public string AspireDbName { get; set; } = string.Empty;
    public string ColumnHashingSecret { get; set; } = string.Empty;
    public bool IsAuthenticationService { get; set; } = false;
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
        InfrastructureSetupOptions options = Validate(configureOptions);
        
        builder.Services.AddScoped<IAuditHistoryService, AuditHistoryService<TDbContext>>();

        builder.ConfigureDatabase<TDbContext>(options);

        builder.Services.AddAuthentication()
            .AddBearerToken(IdentityConstants.BearerScheme);

        builder.Services.AddAuthorizationBuilder();

        builder.Services.AddHttpContextAccessor();

        return builder;
    }

    public static IHostApplicationBuilder AddZenInfrastructure<TDbContext, TUser>(
        this IHostApplicationBuilder builder,
        Action<InfrastructureSetupOptions> configureOptions) 
        where TDbContext : DbContext, IZenDbContext
        where TUser : ZenUser
    {
        AddZenInfrastructure<TDbContext>(builder, configureOptions);

        builder.Services
            .AddIdentityCore<TUser>(identityOptions =>
            {
                identityOptions.Password.RequireDigit = true;
                identityOptions.Password.RequiredLength = 6;
                identityOptions.Lockout.AllowedForNewUsers = true;
            })
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<TDbContext>()
            .AddApiEndpoints();

        builder.Services.AddScoped<UserManager<TUser>>();
        builder.Services.AddScoped<RoleManager<IdentityRole>>();
        return builder;
    }
    private static void ConfigureDatabase<TDbContext>(this IHostApplicationBuilder builder, InfrastructureSetupOptions options) where TDbContext : DbContext, IZenDbContext
    {
        builder.Services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
        builder.Services.AddScoped<ISaveChangesInterceptor, ConvertDomainEventsToOutboxMessagesInterceptor>();

        var connectionString = builder.Configuration.GetConnectionString(options.AspireDbName);

        builder.Services.Configure<ZenDbContextOptions>(opt =>
        {
            opt.ColumnEncryptionService = new AesColumnEncryptionService(options.ColumnHashingSecret);
        });

        builder.Services.AddDbContext<TDbContext>((sp, opt) =>
        {
            opt.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
            opt.UseSqlServer(connectionString, sqlOptions =>
            {
                sqlOptions.MigrationsAssembly(options.Assembly);
            });

        });

        builder.Services.AddDbContextFactory<TDbContext>(opt =>
        {
            opt.UseSqlServer(connectionString);
        }, ServiceLifetime.Scoped);
    }

    private static InfrastructureSetupOptions Validate(Action<InfrastructureSetupOptions> configureOptions)
    {
        InfrastructureSetupOptions options = new();
        configureOptions(options);

        var validator = new InfrastructureSetupOptionsValidator();
        var validationResult = validator.Validate(options);
        if (!validationResult.IsValid)
        {
            var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
            throw new InvalidOperationException($"Invalid options: {errors}");
        }
        return options;
    }

    private sealed class InfrastructureSetupOptionsValidator : AbstractValidator<InfrastructureSetupOptions>
    {
        public InfrastructureSetupOptionsValidator()
        {
            RuleFor(x => x.Assembly)
                .NotEmpty().WithMessage("Assembly is required.");
            RuleFor(x => x.AspireDbName)
                .NotEmpty().WithMessage("AspireDbName is required.");
            RuleFor(x => x.ColumnHashingSecret)
                .NotEmpty().WithMessage("ColumnHashingSecret is required.");
        }
    }
}
