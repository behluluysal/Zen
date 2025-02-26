using Microsoft.EntityFrameworkCore;
using Zen.Services.Coupon.Domain.Repositories;
using Zen.Services.Coupon.Infrastructure.Data;

namespace Zen.Services.Coupon.Infrastructure.Repositories;

public class CouponRepository(CouponDbContext context) : ICouponRepository
{
    private readonly CouponDbContext _context = context;

    public async Task AddAsync(Domain.Entities.Coupon entity, CancellationToken cancellationToken = default)
    {
        await _context.Coupons.AddAsync(entity, cancellationToken);
    }

    public async Task DeleteAsync(Domain.Entities.Coupon entity, CancellationToken cancellationToken = default)
    {
        _context.Coupons.Remove(entity);
        await Task.CompletedTask;
    }

    public async Task<Domain.Entities.Coupon?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        return await _context.Coupons.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    public async Task UpdateAsync(Domain.Entities.Coupon entity, CancellationToken cancellationToken = default)
    {
        _context.Coupons.Update(entity);
        await Task.CompletedTask;
    }

    public async Task<IEnumerable<Domain.Entities.Coupon>> GetValidCouponsAsync(System.DateTime now, CancellationToken cancellationToken = default)
    {
        return await _context.Coupons
            .Where(c => c.Expiration >= now)
            .ToListAsync(cancellationToken);
    }
}