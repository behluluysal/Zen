using Zen.Domain.Repositories;

namespace Zen.Services.Coupon.Domain.Repositories;

public interface ICouponRepository : ICrudRepository<Entities.Coupon>
{
    Task<IEnumerable<Entities.Coupon>> GetValidCouponsAsync(DateTime now, CancellationToken cancellationToken = default);
}