using Zen.API.Extensions;
using Zen.Services.Coupon.Application;
using Zen.Services.Coupon.Infrastructure;
using Zen.Services.Coupon.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.AddServiceDefaults();

builder.AddZenApiDefaults(options =>
{
    options.ConfigureLayers = (configuration) =>
    {
        builder
            .AddCouponApplication(options =>
            {
                options.Assembly = "Zen.Services.Coupon.Application";
                options.Configuration = configuration;
                options.UseAutoMapper = true;
                options.UseFluentValidation = true;
                options.UseMediatR = true;
            })
            .AddCouponInfrastructure(options =>
            {
                options.Assembly = "Zen.Services.Coupon.Infrastructure";
                options.AspireDbName = "coupon-db";
                options.ColumnHashingSecret = configuration["Security:ColumnHashingSecret"];
            });
    };
});

var app = builder.Build();

app.MapDefaultEndpoints();

app.UseZenApiPipeline<CouponDbContext>();

app.Run();
