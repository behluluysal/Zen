namespace Zen.Domain.Events;

public abstract class ZenAuditSnapshot
{
    public string CreatedBy { get; set; } = default!;
    public DateTimeOffset CreatedDate { get; set; }
    public string? UpdatedBy { get; set; }
    public DateTimeOffset? UpdatedDate { get; set; }
}