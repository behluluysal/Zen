using Zen.Domain;

namespace Zen.Services.Coupon.Domain.Repositories;

public interface ICouponRepository : IConcurrencyAwareRepository<Entities.Coupon>
{
    Task<IEnumerable<Entities.Coupon>> GetValidCouponsAsync(DateTime now, CancellationToken cancellationToken = default);
}