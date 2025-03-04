namespace Zen.Domain.Repositories;

/// <summary>
/// Provides generic CRUD operations for aggregate roots.
/// The aggregate root must implement <see cref="IIdentifiable{T}"/> (with T as string) and <see cref="IAggregateMember"/>.
/// </summary>
/// <typeparam name="T">The aggregate root type.</typeparam>
public interface ICrudRepository<T> where T : class, IIdentifiable<string>, IAggregateMember
{
    /// <summary>
    /// Adds a new aggregate to the repository.
    /// </summary>
    /// <param name="entity">The aggregate entity to add.</param>
    /// <param name="cancellationToken">A token that may be used to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task AddAsync(T entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes an aggregate from the repository.
    /// </summary>
    /// <param name="entity">The aggregate entity to delete.</param>
    /// <param name="cancellationToken">A token that may be used to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task DeleteAsync(T entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves an aggregate by its unique identifier (ULID).
    /// </summary>
    /// <param name="id">The ULID of the aggregate.</param>
    /// <param name="cancellationToken">A token that may be used to cancel the asynchronous operation.</param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains the aggregate entity if found; otherwise, <c>null</c>.
    /// </returns>
    Task<T?> GetByIdAsync(string id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an aggregate in the repository.
    /// If the entity implements <see cref="IConcurrencyAware"/>, an original row version must be supplied to enforce optimistic concurrency.
    /// </summary>
    /// <param name="entity">The aggregate entity with updated data.</param>
    /// <param name="originalRowVersion">
    /// The original row version used for concurrency validation.
    /// This parameter is required if the entity implements <see cref="IConcurrencyAware"/>.
    /// </param>
    /// <param name="cancellationToken">A token that may be used to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task UpdateAsync(T entity, byte[]? originalRowVersion = null, CancellationToken cancellationToken = default);
}