using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Zen.Services.Coupon.Application;
using Zen.Services.Coupon.Infrastructure;
using Zen.Services.Coupon.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Logging.AddOpenTelemetry(logging =>
{
    logging.IncludeFormattedMessage = true;
    logging.IncludeScopes = true;
    logging.ParseStateValues = true;
});

builder.AddServiceDefaults();

builder.AddCouponApplication(
    options =>
    {
        options.Assembly = "Zen.Services.Coupon.Application";
        options.Configuration = configuration;
        options.UseAutoMapper = true;
        options.UseFluentValidation = true;
        options.UseMediatR = true;
    });

// Register Coupon Infrastructure services.
builder.AddCouponInfrastructure(
    options =>
    {
        options.Assembly = "Zen.Services.Coupon.Infrastructure";
        options.AspireDbName = "coupon-db";
        //options.ColumnHashingSecret = configuration["Security:ColumnHashingSecret"];
    });

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();


var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

using var scope = app.Services.CreateScope();
var _db = scope.ServiceProvider.GetRequiredService<CouponDbContext>();

if (_db.Database.GetPendingMigrations().Any())
{
    _db.Database.Migrate();
}

app.Run();
