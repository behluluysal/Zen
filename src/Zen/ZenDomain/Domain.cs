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
/// Indicates that an entity has inline audit information.
/// </summary>
public interface IAuditable
{
    string CreatedBy { get; }
    DateTime CreatedDate { get; }
    string? UpdatedBy { get; }
    DateTime? UpdatedDate { get; }
}

/// <summary>
/// Indicates that an entity supports separate audit history tracking.
/// </summary>
/// <typeparam name="THistory">The type representing the audit history record.</typeparam>
public interface IHasAuditHistory<THistory>
{
    ICollection<THistory> AuditHistories { get; set; }
}

/// <summary>
/// Provides a contract for optimistic concurrency management.
/// </summary>
public interface IConcurrencyAware
{
    [Timestamp]
    byte[]? RowVersion { get; set; }
}