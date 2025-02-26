using Microsoft.EntityFrameworkCore;

namespace Zen.Infrastructure.Data;

/// <summary>
/// A base DbContext for Zen applications.
/// Microservices can inherit from this context to gain common configurations,
/// such as optimistic concurrency support and shared conventions.
/// </summary>
public abstract class ZenDbContext(DbContextOptions options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}
