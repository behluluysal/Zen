using Microsoft.Extensions.Hosting;
using Zen.Application.DependencyInjection;

namespace Zen.Services.Coupon.Application;

public static class CouponApplicationServiceBuilder
{
    public static IHostApplicationBuilder AddCouponApplication(this IHostApplicationBuilder builder,
                Action<ApplicationSetupOptions> configureOptions)
    {
        builder.AddZenApplication(configureOptions);

        return builder;
    }
}