using Microsoft.EntityFrameworkCore;
using Zen.Domain.Repositories;
using Zen.Domain.Utilities;
using Zen.Infrastructure.Data;

namespace Zen.Infrastructure.Repositories;

/// <summary>
/// An EF Core implementation of the <see cref="IAuditHistoryRepository"/> interface,
/// providing methods to manage audit history records.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="AuditHistoryRepository"/> class.
/// </remarks>
/// <param name="context">The database context to use for data access.</param>
internal sealed class AuditHistoryRepository<TDbContext>(TDbContext context) 
    : IAuditHistoryRepository where TDbContext : DbContext, IZenDbContext
{
    /// <inheritdoc/>
    public async Task AddAsync(AuditHistoryRecord auditHistoryRecord, CancellationToken cancellationToken = default)
    {
        await context.Set<AuditHistoryRecord>().AddAsync(auditHistoryRecord, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<AuditHistoryRecord>> GetAuditHistoryAsync(string entityId, CancellationToken cancellationToken = default)
    {
        return await context.Set<AuditHistoryRecord>()
            .Where(a => a.EntityId == entityId)
            .ToListAsync(cancellationToken);
    }
}