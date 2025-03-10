using Zen.Domain.Events;

namespace Zen.Domain.Aggregates;

public abstract class AggregateRoot : AggregateMember
{

}

#region [ AggregateMember ]

public abstract class AggregateMember : IAggregateMember
{
    private readonly List<IDomainEvent> _domainEvents = [];

    public IReadOnlyCollection<IDomainEvent> GetDomainEvents() => [.. _domainEvents];
    protected void Raise(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);
    public void ClearDomainEvents() => _domainEvents.Clear();
}

/// <summary>
/// Marker interface to denote an aggregate member.
/// </summary>
public interface IAggregateMember
{
    void ClearDomainEvents();
    IReadOnlyCollection<IDomainEvent> GetDomainEvents();
}

#endregion
