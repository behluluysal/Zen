namespace Zen.Application.Messaging.Event;

/// <summary>
/// Contract for dispatching domain events.
/// </summary>
public interface IEventDispatcher
{
    Task DispatchAsync(DomainEventWrapper domainEvent, CancellationToken cancellationToken = default);
}

/// <summary>
/// Wrapper to hold a domain event.
/// You might integrate this with your domain event infrastructure.
/// </summary>
public class DomainEventWrapper
{
    public required object DomainEvent { get; set; }
    public DateTime OccurredOn { get; set; } = DateTime.UtcNow;
}