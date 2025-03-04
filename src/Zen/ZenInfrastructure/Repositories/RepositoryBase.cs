using Microsoft.EntityFrameworkCore;
using Zen.Domain;
using Zen.Domain.Repositories;

namespace Zen.Infrastructure.Repositories;


/// <summary>
/// Provides a base implementation of the <see cref="ICrudRepository{T}"/> interface,
/// encapsulating common CRUD operations for aggregate roots.
/// </summary>
/// <typeparam name="T">The aggregate root type.</typeparam>
public abstract class RepositoryBase<T> : ICrudRepository<T> where T : class, IIdentifiable<string>, IAggregateMember
{
    protected readonly DbContext _context;
    protected readonly DbSet<T> _dbSet;

    protected RepositoryBase(DbContext context)
    {
        _context = context;
        _dbSet = _context.Set<T>();
    }

    /// <inheritdoc/>
    public virtual async Task<T?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        return await _dbSet.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
    }

    /// <inheritdoc/>
    public virtual async Task AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        await _dbSet.AddAsync(entity, cancellationToken);
    }

    /// <inheritdoc/>
    public virtual async Task DeleteAsync(T entity, CancellationToken cancellationToken = default)
    {
        _dbSet.Remove(entity);
        await Task.CompletedTask;
    }

    /// <inheritdoc/>
    public virtual async Task UpdateAsync(T entity, byte[]? originalRowVersion = null, CancellationToken cancellationToken = default)
    {
        var entry = _context.Entry(entity);

        if (entity is IConcurrencyAware)
        {
            if (originalRowVersion == null)
            {
                throw new ArgumentNullException(nameof(originalRowVersion), "A row version must be provided for concurrency-aware entities.");
            }
            entry.Property("RowVersion").OriginalValue = originalRowVersion;
        }

        entry.State = EntityState.Modified;
        await Task.CompletedTask;
    }
}