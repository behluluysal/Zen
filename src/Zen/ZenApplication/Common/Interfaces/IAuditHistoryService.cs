using Zen.Domain.Auditing;

namespace Zen.Application.Common.Interfaces;

public interface IAuditHistoryService
{
    Task LogAuditAsync(AuditHistoryRecord record, CancellationToken cancellationToken = default);
    Task<IEnumerable<AuditHistoryRecord>> GetAuditHistoryAsync(string entityId, CancellationToken cancellationToken = default);
}