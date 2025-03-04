using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Zen.API.HangFire;
using Zen.API.Middleware;
using Zen.Infrastructure.BackgroundJobs;
using Zen.Infrastructure.Data;

namespace Zen.API.Extensions;

public static class ZenApiPipelineExtensions
{
    /// <summary>
    /// Configures a common API pipeline for Zen-based microservices.
    /// </summary>
    /// <param name="app">The application builder.</param>
    /// <returns>The updated application builder.</returns>
    public static IApplicationBuilder UseZenApiPipeline<TDbContext>(this WebApplication app) 
        where TDbContext : DbContext, IZenDbContext
    {
        var jobs = app.Services.GetRequiredService<ZenJobScheduler<TDbContext>>();
        jobs.ScheduleRecurringJobs();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();

            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/openapi/v1.json", "OpenAPI v1");
            });
        }

        app.UseMiddleware<ZenUserContextMiddleware>();

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        using var scope = app.Services.CreateScope();
        var _db = scope.ServiceProvider.GetRequiredService<TDbContext>();

        if (_db.Database.GetPendingMigrations().Any())
        {
            _db.Database.Migrate();
        }

        app.UseHangfireDashboard("/jobs", new DashboardOptions
        {
            Authorization = [new HangfireDashboardAuthorizationFilter()],
            AsyncAuthorization = [new HangfireDashboardAsyncAuthorizationFilter()]
        });

        return app;
    }
}