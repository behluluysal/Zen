using Microsoft.EntityFrameworkCore;

namespace Zen.Services.Coupon.Application;

public interface ICouponDbContext
{
    DbSet<Domain.CouponAggregate.Coupon> Coupons { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
