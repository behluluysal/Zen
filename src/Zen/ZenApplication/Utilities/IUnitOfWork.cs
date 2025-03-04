namespace Zen.Application.Utilities.Transaction;

/// <summary>
/// Unit of Work abstraction to manage transactional operations.
/// </summary>
public interface IUnitOfWork
{
    /// <summary>
    /// Begins a new transaction.
    /// </summary>
    /// <param name="cancellationToken">Optional cancellation token.</param>
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Commits the current transaction.
    /// </summary>
    /// <param name="cancellationToken">Optional cancellation token.</param>
    Task CommitAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Rolls back the current transaction.
    /// </summary>
    /// <param name="cancellationToken">Optional cancellation token.</param>
    Task RollbackAsync(CancellationToken cancellationToken = default);
}