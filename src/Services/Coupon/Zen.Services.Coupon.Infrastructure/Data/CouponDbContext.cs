using Microsoft.EntityFrameworkCore;
using Zen.Infrastructure.Data;

namespace Zen.Services.Coupon.Infrastructure.Data;

public class CouponDbContext(DbContextOptions<CouponDbContext> options) : ZenDbContext(options)
{
    public DbSet<Domain.Entities.Coupon> Coupons { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Domain.Entities.Coupon>(entity =>
        {
            entity.Property(c => c.RowVersion).IsRowVersion();
        });
    }
}