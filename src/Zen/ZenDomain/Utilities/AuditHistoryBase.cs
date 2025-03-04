namespace Zen.Domain.Utilities;

public class AuditHistoryRecord : AuditHistoryBase
{
}

/// <summary>
/// Defines the base properties for audit history records.
/// Microservices can extend this class to persist history for any audited entity.
/// </summary>
public abstract class AuditHistoryBase
{
    public string Id { get; set; } = Ulid.NewUlid().ToString();

    public required string EntityId { get; set; }

    public AuditOperation Operation { get; set; }

    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    public required string Snapshot { get; set; }

    public required string ChangedBy { get; set; }
}

/// <summary>
/// Enumerates the types of operations that can be audited.
/// </summary>
public enum AuditOperation
{
    Insert,
    Update,
    Delete
}