using MediatR;

namespace Zen.Domain.Events;

/// <summary>
/// Represents a domain event.
/// Domain events capture important state changes or side effects in the domain.
/// </summary>
public interface IDomainEvent : INotification
{
}

/// <summary>
/// Provides a default OccurredOn timestamp.
/// </summary>
public abstract record ZenDomainEvent : IDomainEvent
{
    public DateTime OccurredOn { get; protected set; } = DateTime.UtcNow;
}