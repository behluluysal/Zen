namespace Zen.Domain.Outbox;

public class OutboxMessage
{
    public string Id { get; set; } = Ulid.NewUlid().ToString();

    public string Type { get; set; } = string.Empty;

    public string Content { get; set; } = string.Empty;

    public DateTime OccurredOnUtc { get; set; }

    public DateTime? ProcessedOnUtc { get; set; }

    public string? Error { get; set; }

    public int RetryCount { get; set; } = 0;
}