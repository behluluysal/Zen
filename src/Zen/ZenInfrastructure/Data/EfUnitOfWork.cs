using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Zen.Application.Utilities.Transaction;

namespace Zen.Infrastructure.Data;

/// <summary>
/// EF Core–based Unit of Work implementation.
/// Manages transactions using the provided DbContext.
/// </summary>
/// <typeparam name="TContext">The type of the DbContext (must inherit from DbContext).</typeparam>
public class EfUnitOfWork<TContext>(TContext context) : IUnitOfWork where TContext : DbContext
{
    private readonly TContext _context = context;
    private IDbContextTransaction? _transaction;

    /// <summary>
    /// Begins a new database transaction.
    /// </summary>
    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        _transaction ??= await _context.Database.BeginTransactionAsync(cancellationToken);
    }

    /// <summary>
    /// Saves all changes and commits the current transaction.
    /// </summary>
    public async Task CommitAsync(CancellationToken cancellationToken = default)
    {
        // Save the changes to the database.
        await _context.SaveChangesAsync(cancellationToken);

        if (_transaction != null)
        {
            await _transaction.CommitAsync(cancellationToken);
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    /// <summary>
    /// Rolls back the current transaction.
    /// </summary>
    public async Task RollbackAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync(cancellationToken);
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }
}