using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Zen.Domain.Auditing;
using Zen.Infrastructure.Data;
using Zen.Services.Sso.Application;
using Zen.Services.Sso.Domain.UserAggregate;

namespace Zen.Services.Sso.Infrastructure.Data;

public class SsoDbContext(DbContextOptions<SsoDbContext> options, IOptions<ZenDbContextOptions> zenOptions) 
    : ZenIdentityDbContext<AppUser>(options, zenOptions), ISsoDbContext
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<IdentityUser>(b =>
        {
            b.HasKey(u => u.Id); // Ensure the key is configured on the root type
            b.Property(u => u.Id).HasMaxLength(128); // Example: Customize the Id property
        });
        modelBuilder.Entity<AuditHistoryRecord>(entity =>
        {
            entity.HasOne<AppUser>()
                  .WithMany()
                  .HasForeignKey(a => a.EntityId)
                  .HasPrincipalKey(c => c.Id)
                  .OnDelete(DeleteBehavior.Cascade);
        });
    }
}