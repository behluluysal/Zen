using Microsoft.EntityFrameworkCore;
using Zen.Domain.Auditing;
using Zen.Infrastructure.Data;
using Zen.Services.Coupon.Application;

namespace Zen.Services.Coupon.Infrastructure.Data;

public class CouponDbContext(DbContextOptions<CouponDbContext> options) : ZenDbContext(options), ICouponDbContext
{
    public DbSet<Domain.Entities.CouponAggregate.Coupon> Coupons { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Domain.Entities.CouponAggregate.Coupon>(entity =>
        {
            entity.Property(c => c.RowVersion).IsRowVersion();
        });

        modelBuilder.Entity<AuditHistoryRecord>(entity =>
        {
            entity.HasOne<Domain.Entities.CouponAggregate.Coupon>()
                  .WithMany()
                  .HasForeignKey(a => a.EntityId)
                  .HasPrincipalKey(c => c.Id)
                  .OnDelete(DeleteBehavior.Cascade);
        });
    }
}