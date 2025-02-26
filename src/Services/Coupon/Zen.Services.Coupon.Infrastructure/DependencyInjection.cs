using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Zen.Infrastructure.DependencyInjection;
using Zen.Services.Coupon.Domain.Repositories;
using Zen.Services.Coupon.Infrastructure.Data;
using Zen.Services.Coupon.Infrastructure.Repositories;

namespace Zen.Services.Coupon.Infrastructure;

public static class CouponInfrastructureServiceBuilder
{
    public static IHostApplicationBuilder AddCouponInfrastructure(this IHostApplicationBuilder builder, 
        Action<InfrastructureSetupOptions> configureOptions)
    {
        builder.AddZenInfrastructure<CouponDbContext>(configureOptions);

        builder.Services.AddScoped<ICouponRepository, CouponRepository>();

        return builder;
    }
}