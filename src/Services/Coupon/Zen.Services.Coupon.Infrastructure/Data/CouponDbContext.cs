using Microsoft.EntityFrameworkCore;
using Zen.Domain.Auditing;
using Zen.Infrastructure.Data;
using Zen.Services.Coupon.Application;

namespace Zen.Services.Coupon.Infrastructure.Data;

public class CouponDbContext(DbContextOptions<CouponDbContext> options) : ZenDbContext(options), ICouponDbContext
{
    public DbSet<Domain.Entities.Coupon> Coupons { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Domain.Entities.Coupon>(entity =>
        {
            entity.Property(c => c.RowVersion).IsRowVersion();
        });

        modelBuilder.Entity<AuditHistoryRecord>(entity =>
        {
            entity.HasOne<Domain.Entities.Coupon>()
                  .WithMany()
                  .HasForeignKey(a => a.EntityId)
                  .HasPrincipalKey(c => c.Id)
                  .OnDelete(DeleteBehavior.Cascade);
        });
    }
}