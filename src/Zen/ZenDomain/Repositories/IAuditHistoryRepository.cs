using Zen.Domain.Utilities;

namespace Zen.Domain.Repositories;

/// <summary>
/// Provides methods for managing audit history records.
/// </summary>
public interface IAuditHistoryRepository
{
    /// <summary>
    /// Adds a new audit history record to the repository.
    /// </summary>
    /// <param name="auditHistoryRecord">The audit history record to add.</param>
    /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous add operation.</returns>
    Task AddAsync(AuditHistoryRecord auditHistoryRecord, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves all audit history records associated with the specified entity identifier.
    /// </summary>
    /// <param name="entityId">The unique identifier of the audited entity.</param>
    /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
    /// <returns>
    /// A task that represents the asynchronous retrieval operation.
    /// The task result contains an enumerable collection of audit history records.
    /// </returns>
    Task<IEnumerable<AuditHistoryRecord>> GetAuditHistoryAsync(string entityId, CancellationToken cancellationToken = default);
}