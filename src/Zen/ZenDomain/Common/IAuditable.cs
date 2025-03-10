namespace Zen.Domain.Common;

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