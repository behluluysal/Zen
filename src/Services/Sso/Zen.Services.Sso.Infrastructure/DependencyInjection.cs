using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Zen.Infrastructure.DependencyInjection;
using Zen.Services.Sso.Application;
using Zen.Services.Sso.Application.Services;
using Zen.Services.Sso.Domain.UserAggregate;
using Zen.Services.Sso.Infrastructure.Auth;
using Zen.Services.Sso.Infrastructure.Data;

namespace Zen.Services.Sso.Infrastructure;

public static class SsoInfrastructureServiceBuilder
{
    public static IHostApplicationBuilder AddSsoInfrastructure(this IHostApplicationBuilder builder,
        Action<InfrastructureSetupOptions> configureOptions)
    {
        builder.AddZenInfrastructure<SsoDbContext, AppUser>(configureOptions);
        builder.Services.AddScoped<ISsoDbContext>(provider => provider.GetRequiredService<SsoDbContext>());

        var jwtSection = builder.Configuration.GetSection("Jwt");
        builder.Services.Configure<JwtSettings>(jwtSection);

        builder.Services.AddScoped<IZenTokenService, ZenTokenService>();

        return builder;
    }
}