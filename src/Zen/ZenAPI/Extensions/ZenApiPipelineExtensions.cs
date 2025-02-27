using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Zen.Infrastructure.Data;

namespace Zen.API.Extensions;

public static class ZenApiPipelineExtensions
{
    /// <summary>
    /// Configures a common API pipeline for Zen-based microservices.
    /// </summary>
    /// <param name="app">The application builder.</param>
    /// <returns>The updated application builder.</returns>
    public static IApplicationBuilder UseZenApiPipeline<TDbContext>(this WebApplication app) where TDbContext : ZenDbContext
    {
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        using var scope = app.Services.CreateScope();
        var _db = scope.ServiceProvider.GetRequiredService<TDbContext>();

        if (_db.Database.GetPendingMigrations().Any())
        {
            _db.Database.Migrate();
        }

        return app;
    }
}