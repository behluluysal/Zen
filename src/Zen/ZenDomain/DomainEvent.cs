namespace Zen.Domain.DomainEvent;

/// <summary>
/// Represents a domain event.
/// Domain events capture important state changes or side effects in the domain.
/// </summary>
public interface IDomainEvent
{
    DateTime OccurredOn { get; }
}

/// <summary>
/// Provides a default OccurredOn timestamp.
/// </summary>
public abstract class DomainEvent : IDomainEvent
{
    public DateTime OccurredOn { get; protected set; } = DateTime.UtcNow;
}

/// <summary>
/// Contract to coordinate domain events.
/// </summary>
public interface IDomainEventDispatcher
{
    Task DispatchAsync(IDomainEvent domainEvent, CancellationToken cancellationToken = default);
}