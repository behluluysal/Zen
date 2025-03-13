using Microsoft.EntityFrameworkCore;

namespace Zen.Services.Coupon.Application;

public interface ICouponDbContext
{
    DbSet<Domain.Entities.CouponAggregate.Coupon> Coupons { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
