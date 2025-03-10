using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Zen.Infrastructure.DependencyInjection;
using Zen.Services.Coupon.Application;
using Zen.Services.Coupon.Infrastructure.Data;

namespace Zen.Services.Coupon.Infrastructure;

public static class CouponInfrastructureServiceBuilder
{
    public static IHostApplicationBuilder AddCouponInfrastructure(this IHostApplicationBuilder builder, 
        Action<InfrastructureSetupOptions> configureOptions)
    {
        builder.AddZenInfrastructure<CouponDbContext>(configureOptions);
        builder.Services.AddScoped<ICouponDbContext>(provider => provider.GetRequiredService<CouponDbContext>());

        return builder;
    }
}