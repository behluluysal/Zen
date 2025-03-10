using System.ComponentModel.DataAnnotations;

namespace Zen.Domain.Common;

/// <summary>
/// Provides a contract for optimistic concurrency management.
/// </summary>
public interface IConcurrencyAware
{
    [Timestamp]
    byte[]? RowVersion { get; set; }
}