using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Text.Json;
using Zen.Domain.Aggregates;
using Zen.Domain.Outbox;

namespace Zen.Infrastructure.Data.Interceptors;

public sealed class ConvertDomainEventsToOutboxMessagesInterceptor
    : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        SaveDomainEvents(eventData.Context);

        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        SaveDomainEvents(eventData.Context);

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private static void SaveDomainEvents(DbContext? dbContext)
    {
        if (dbContext is null)
        {
            return;
        }

        var outboxMessages = dbContext.ChangeTracker
            .Entries<AggregateRoot>()
            .Select(x => x.Entity)
            .SelectMany(aggregateRoot =>
            {
                var domainEvents = aggregateRoot.GetDomainEvents();
                aggregateRoot.ClearDomainEvents();
                return domainEvents;
            })
            .Select(domainEvent => new OutboxMessage
            {
                OccurredOnUtc = DateTime.UtcNow,
                Type = domainEvent.GetType().AssemblyQualifiedName ?? string.Empty,
                Content = JsonSerializer.Serialize(domainEvent, domainEvent.GetType())
            })
            .ToList();

        dbContext.Set<OutboxMessage>().AddRange(outboxMessages);
    }
}