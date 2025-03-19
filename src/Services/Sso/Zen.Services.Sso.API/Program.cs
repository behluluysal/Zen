using Zen.API.Extensions;
using Zen.ServiceDefaults;
using Zen.Services.Sso.Application;
using Zen.Services.Sso.Infrastructure;
using Zen.Services.Sso.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.AddServiceDefaults();

builder.AddZenApiDefaults<SsoDbContext>(options =>
{
    options.ConfigureLayers = (configuration) =>
    {
        builder
            .AddSsoApplication(options =>
            {
                options.Assembly = "Zen.Services.Sso.Application";
                options.Configuration = configuration;
                options.UseAutoMapper = true;
                options.UseFluentValidation = true;
                options.UseMediatR = true;
            })
            .AddSsoInfrastructure(options =>
            {
                options.Assembly = "Zen.Services.Sso.Infrastructure";
                options.AspireDbName = "sso-db";
                options.ColumnHashingSecret = configuration["Security:ColumnHashingSecret"];
            });
    };
});

var app = builder.Build();

app.MapDefaultEndpoints();

app.UseZenApiPipeline<SsoDbContext>();

app.Run();
