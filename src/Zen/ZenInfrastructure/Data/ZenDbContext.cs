using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Zen.Domain.Auditing;
using Zen.Domain.Outbox;
using Zen.Infrastructure.Data.Extensions;
using Zen.Infrastructure.Data.Security;

namespace Zen.Infrastructure.Data;

public class ZenDbContextOptions
{
    public IColumnEncryptionService ColumnEncryptionService { get; set; } = default!;
}

/// <summary>
/// A base DbContext for Zen applications.
/// Microservices can inherit from this context to gain common configurations,
/// such as optimistic concurrency support and shared conventions.
/// </summary>
public abstract class ZenDbContext(DbContextOptions options, IOptions<ZenDbContextOptions> zenOptions) : DbContext(options), IZenDbContext
{
    private readonly ZenDbContextOptions _zenOptions = zenOptions.Value;
    public DbSet<OutboxMessage> OutboxMessages { get; set; }
    public DbSet<AuditHistoryRecord> AuditHistoryRecords { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ConfigureZenModel(_zenOptions.ColumnEncryptionService);
    }
}

/// <summary>
/// A base IdentityDbContext for Zen applications.
/// This class extends IdentityDbContext to include common configurations,
/// such as encryption for sensitive properties and optimistic concurrency support.
/// Microservices handling authentication can inherit from this context.
/// </summary>
public abstract class ZenIdentityDbContext(DbContextOptions options, IOptions<ZenDbContextOptions> zenOptions) : IdentityDbContext(options), IZenDbContext
{
    private readonly ZenDbContextOptions _zenOptions = zenOptions.Value;
    public DbSet<OutboxMessage> OutboxMessages { get; set; }
    public DbSet<AuditHistoryRecord> AuditHistoryRecords { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ConfigureZenModel(_zenOptions.ColumnEncryptionService);
    }
}

public interface IZenDbContext
{
    DbSet<OutboxMessage> OutboxMessages { get; set; }
    DbSet<AuditHistoryRecord> AuditHistoryRecords { get; set; }
}
