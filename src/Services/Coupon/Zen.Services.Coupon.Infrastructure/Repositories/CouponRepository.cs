using Microsoft.EntityFrameworkCore;
using Zen.Infrastructure.Repositories;
using Zen.Services.Coupon.Domain.Repositories;
using Zen.Services.Coupon.Infrastructure.Data;

namespace Zen.Services.Coupon.Infrastructure.Repositories;

public class CouponRepository(CouponDbContext context) 
    : RepositoryBase<Domain.Entities.Coupon>(context), ICouponRepository
{
    public async Task<IEnumerable<Domain.Entities.Coupon>> GetValidCouponsAsync(System.DateTime now, CancellationToken cancellationToken = default)
    {
        return await context.Coupons
            .Where(c => c.Expiration >= now)
            .ToListAsync(cancellationToken);
    }
}