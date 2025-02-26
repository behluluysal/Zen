using System.ComponentModel.DataAnnotations;

namespace Zen.Domain;

/// <summary>
/// Represents an entity with an identifier.
/// Using a string for ULIDs.
/// </summary>
public interface IIdentifiable<T>
{
    T Id { get; }
}

/// <summary>
/// Marker interface to denote an aggregate root.
/// </summary>
public interface IAggregateRoot { }

///// <summary>
///// Indicates that an entity has inline audit information.
///// This is useful if you need quick access to audit data.
///// </summary>
//public interface IAuditable
//{
//    string CreatedBy { get; }
//    DateTime CreatedDate { get; }
//    string UpdatedBy { get; }
//    DateTime? UpdatedDate { get; }
//}

///// <summary>
///// Indicates that an entity supports separate audit history tracking.
///// </summary>
///// <typeparam name="THistory">The type representing the audit history record.</typeparam>
//public interface IHasAuditHistory<THistory>
//{
//    ICollection<THistory> AuditHistories { get; set; }
//}

/// <summary>
/// Provides a contract for optimistic concurrency management.
/// </summary>
public interface IConcurrencyAware
{
    [Timestamp]
    byte[]? RowVersion { get; set; }
}




/// <summary>
/// Repository interface for aggregate roots.
/// This abstraction lets the domain layer remain decoupled from data access details.
/// </summary>
/// <typeparam name="T">
/// The type of the aggregate root.
/// It must be identifiable by a ULID (string) and be marked as an aggregate root.
/// </typeparam>
public interface IRepository<T> where T : IIdentifiable<string>, IAggregateRoot
{
    Task<T?> GetByIdAsync(string id, CancellationToken cancellationToken = default);
    Task AddAsync(T entity, CancellationToken cancellationToken = default);
    Task UpdateAsync(T entity, CancellationToken cancellationToken = default);
    Task DeleteAsync(T entity, CancellationToken cancellationToken = default);
}

