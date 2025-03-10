using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Zen.Domain.Auditing;
using Zen.Domain.Outbox;
using Zen.Infrastructure.Data.Extensions;
using Zen.Infrastructure.Data.Security;

namespace Zen.Infrastructure.Data;

/// <summary>
/// A base DbContext for Zen applications.
/// Microservices can inherit from this context to gain common configurations,
/// such as optimistic concurrency support and shared conventions.
/// </summary>
public abstract class ZenDbContext(DbContextOptions options) : DbContext(options), IZenDbContext
{
    public static IColumnEncryptionService? StaticColumnEncryptionService { get; set; }
    public virtual IColumnEncryptionService? ColumnEncryptionService => StaticColumnEncryptionService;
    public DbSet<OutboxMessage> OutboxMessages { get; set; }
    public DbSet<AuditHistoryRecord> AuditHistoryRecords { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ConfigureZenModel(ColumnEncryptionService);
    }
}

/// <summary>
/// A base IdentityDbContext for Zen applications.
/// This class extends IdentityDbContext to include common configurations,
/// such as encryption for sensitive properties and optimistic concurrency support.
/// Microservices handling authentication can inherit from this context.
/// </summary>
/// <typeparam name="TUser">The type of the user entity.</typeparam>
public abstract class ZenIdentityDbContext(DbContextOptions options) : IdentityDbContext(options), IZenDbContext
{
    public static IColumnEncryptionService? StaticColumnEncryptionService { get; set; }
    public virtual IColumnEncryptionService? ColumnEncryptionService => StaticColumnEncryptionService;
    public DbSet<OutboxMessage> OutboxMessages { get; set; }
    public DbSet<AuditHistoryRecord> AuditHistoryRecords { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ConfigureZenModel(ColumnEncryptionService);
    }
}

public interface IZenDbContext
{
    DbSet<OutboxMessage> OutboxMessages { get; set; }
    DbSet<AuditHistoryRecord> AuditHistoryRecords { get; set; }
    IColumnEncryptionService? ColumnEncryptionService { get; }
}
