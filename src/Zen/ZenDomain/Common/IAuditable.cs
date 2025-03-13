namespace Zen.Domain.Common;

/// <summary>
/// Indicates that an entity has inline audit information.
/// </summary>
public interface IAuditable
{
    string CreatedBy { get; }
    DateTimeOffset CreatedDate { get; }
    string? UpdatedBy { get; }
    DateTimeOffset? UpdatedDate { get; }
}