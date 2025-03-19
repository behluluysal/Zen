using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Zen.Domain.Auditing;
using Zen.Infrastructure.Data;
using Zen.Services.Coupon.Application;

namespace Zen.Services.Coupon.Infrastructure.Data;

public class CouponDbContext(DbContextOptions<CouponDbContext> options, IOptions<ZenDbContextOptions> zenOptions) : ZenDbContext(options, zenOptions), ICouponDbContext
{
    public DbSet<Domain.CouponAggregate.Coupon> Coupons { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Domain.CouponAggregate.Coupon>(entity =>
        {
            entity.Property(c => c.RowVersion).IsRowVersion();
        });

        modelBuilder.Entity<AuditHistoryRecord>(entity =>
        {
            entity.HasOne<Domain.CouponAggregate.Coupon>()
                  .WithMany()
                  .HasForeignKey(a => a.EntityId)
                  .HasPrincipalKey(c => c.Id)
                  .OnDelete(DeleteBehavior.Cascade);
        });
    }
}