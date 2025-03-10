using Microsoft.EntityFrameworkCore;
using Zen.Application.Common.Interfaces;
using Zen.Domain.Auditing;
using Zen.Infrastructure.Data;

namespace Zen.Infrastructure.Services;

public class AuditHistoryService<TDbContext>(TDbContext dbContext) 
    : IAuditHistoryService where TDbContext : DbContext, IZenDbContext
{
    public async Task LogAuditAsync(AuditHistoryRecord record, CancellationToken cancellationToken = default)
    {
        await dbContext.Set<AuditHistoryRecord>().AddAsync(record, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<IEnumerable<AuditHistoryRecord>> GetAuditHistoryAsync(string entityId, CancellationToken cancellationToken = default)
    {
        return await dbContext.Set<AuditHistoryRecord>()
                             .Where(a => a.EntityId == entityId)
                             .ToListAsync(cancellationToken);
    }
}