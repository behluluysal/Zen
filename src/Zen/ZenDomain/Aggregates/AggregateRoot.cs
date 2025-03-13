using System.Text.Json.Serialization;
using Zen.Domain.Common;
using Zen.Domain.Events;

namespace Zen.Domain.Aggregates;

public abstract class AggregateRoot : AggregateMember
{

}

public abstract class AuditableAggregateRoot : AggregateMember, IAuditable, IIdentifiable<string>
{
    [JsonInclude]
    public string Id { get; private set; } = Ulid.NewUlid().ToString();
    
    [JsonInclude]
    public string CreatedBy { get; private set; } = string.Empty;
    [JsonInclude]
    public DateTimeOffset CreatedDate { get; private set; } = default;
    [JsonInclude]
    public string? UpdatedBy { get; private set; }
    [JsonInclude]
    public DateTimeOffset? UpdatedDate { get; private set; }
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
