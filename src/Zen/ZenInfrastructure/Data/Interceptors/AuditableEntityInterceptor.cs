using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Zen.Application.Common.Interfaces;
using Zen.Domain.Common;

namespace Zen.Infrastructure.Data.Interceptors;

internal sealed class AuditableEntityInterceptor(
    IZenUserContext userContext,
    TimeProvider dateTime) : SaveChangesInterceptor
{
    private readonly IZenUserContext _userContext = userContext;
    private readonly TimeProvider _dateTime = dateTime;

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        UpdateEntities(eventData.Context);

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    public void UpdateEntities(DbContext? context)
    {
        if (context == null) return;

        foreach (var entry in context.ChangeTracker.Entries<IAuditable>())
        {
            if (entry.State is EntityState.Added or EntityState.Modified || entry.HasChangedOwnedEntities())
            {
                var utcNow = _dateTime.GetUtcNow();
                if (entry.State == EntityState.Added)
                {
                    entry.Property(nameof(entry.Entity.CreatedBy)).CurrentValue = _userContext.UserId;
                    entry.Property(nameof(entry.Entity.CreatedDate)).CurrentValue = utcNow;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Property(nameof(entry.Entity.CreatedBy)).IsModified = false;
                    entry.Property(nameof(entry.Entity.CreatedDate)).IsModified = false;
                    entry.Property(nameof(entry.Entity.UpdatedDate)).CurrentValue = DateTimeOffset.UtcNow;
                    entry.Property(nameof(entry.Entity.UpdatedBy)).CurrentValue = _userContext.UserId;
                }
            }
        }
    }
}

internal static class Extensions
{
    internal static bool HasChangedOwnedEntities(this EntityEntry entry) =>
        entry.References.Any(r =>
            r.TargetEntry != null &&
            r.TargetEntry.Metadata.IsOwned() &&
            (r.TargetEntry.State == EntityState.Added || r.TargetEntry.State == EntityState.Modified));
}