using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.ComponentModel.DataAnnotations;
using Zen.Application.Utilities.Transaction;
using Zen.Infrastructure.Data;
using Zen.Infrastructure.Data.Security;

namespace Zen.Infrastructure.DependencyInjection;

public class InfrastructureSetupOptions
{
#nullable disable
    [Required]
    public string Assembly { get; set; }

    [Required]
    public string AspireDbName { get; set; }

    //[Required]
    //public string ColumnHashingSecret { get; set; }
#nullable enable
}

public static class InfrastructureServiceBuilder
{
    public static IHostApplicationBuilder AddZenInfrastructure<TDbContext>(
        this IHostApplicationBuilder builder,
        Action<InfrastructureSetupOptions> configureOptions) where TDbContext : ZenDbContext
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

        builder.AddSqlServerDbContext<TDbContext>(
            options.AspireDbName,
            configureDbContextOptions: opt =>
            {
                opt.UseSqlServer(sqlOptions =>
                {
                    sqlOptions.MigrationsAssembly(options.Assembly);
                });
            }
        );

        builder.Services.AddScoped<IUnitOfWork, EfUnitOfWork<TDbContext>>();
        //builder.Services.AddSingleton<IDbColumnHasher>(new HmacSha256DbColumnHasher(options.ColumnHashingSecret));


        return builder;
    }
}
