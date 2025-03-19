using Microsoft.Extensions.Hosting;
using Zen.Application.DependencyInjection;

namespace Zen.Services.Sso.Application;

public static class SsoApplicationServiceBuilder
{
    public static IHostApplicationBuilder AddSsoApplication(this IHostApplicationBuilder builder,
                Action<ApplicationSetupOptions> configureOptions)
    {
        builder.AddZenApplication(configureOptions);

        return builder;
    }
}