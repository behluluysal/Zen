using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Zen.Domain.Common;

/// <summary>
/// Provides a contract for optimistic concurrency management.
/// </summary>
public interface IConcurrencyAware
{
    [Timestamp]
    [JsonIgnore]
    byte[]? RowVersion { get; }
}