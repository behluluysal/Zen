using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Zen.Domain.Common;
using Zen.Domain.Events;

namespace Zen.Domain.Aggregates;

[method: JsonConstructor]
public abstract class ZenUser() : IdentityUser, IAggregateMember, IAuditable, ISoftDeletable, IConcurrencyAware
{
    #region [ AggregateRoot ]

    [JsonInclude]
    public override string Id { get; set; } = Ulid.NewUlid().ToString();

    [JsonInclude]
    public string CreatedBy { get; protected set; } = string.Empty;
    [JsonInclude]
    public DateTimeOffset CreatedDate { get; protected set; } = default;
    [JsonInclude]
    public string? UpdatedBy { get; protected set; }
    [JsonInclude]
    public DateTimeOffset? UpdatedDate { get; protected set; }


    private readonly List<IDomainEvent> _domainEvents = [];

    public IReadOnlyCollection<IDomainEvent> GetDomainEvents() => [.. _domainEvents];
    protected void Raise(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);
    public void ClearDomainEvents() => _domainEvents.Clear();

    #endregion

    public bool IsDeleted { get; protected set; }

    [Timestamp]
    [JsonIgnore]
    public byte[]? RowVersion { get; protected set; }
}