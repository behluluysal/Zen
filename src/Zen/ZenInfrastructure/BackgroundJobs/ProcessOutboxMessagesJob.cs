using Hangfire;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Zen.Domain;
using Zen.Domain.Outbox;
using Zen.Infrastructure.Data;

namespace Zen.Infrastructure.BackgroundJobs;

/// <summary>
/// A Hangfire background job for processing outbox messages.
/// This job retrieves up to 10 pending outbox messages, attempts to deserialize and publish each one,
/// and updates their state accordingly. Disables concurrent execution.
/// </summary>
/// <typeparam name="TContext">A DbContext that implements IZenDbContext.</typeparam>
/// <remarks>
/// Initializes a new instance of the <see cref="ProcessOutboxMessagesJob{TContext}"/> class.
/// </remarks>
/// <param name="dbContextFactory">The database context Factory.</param>
/// <param name="publisher">The domain event publisher.</param>
/// <param name="logger">The logger instance.</param>
[DisableConcurrentExecution(60)]
public class ProcessOutboxMessagesJob<TContext>(IDbContextFactory<TContext> dbContextFactory, IPublisher publisher, ILogger<ProcessOutboxMessagesJob<TContext>> logger) 
    where TContext : DbContext, IZenDbContext
{
    private readonly IDbContextFactory<TContext> _dbContextFactory = dbContextFactory;
    private readonly IPublisher _publisher = publisher;
    private readonly ILogger<ProcessOutboxMessagesJob<TContext>> _logger = logger;

    public async Task Execute()
    {
        using var dbContext = await _dbContextFactory.CreateDbContextAsync();

        List<OutboxMessage> messages = await dbContext.Set<OutboxMessage>()
            .Where(m => m.ProcessedOnUtc == null && m.RetryCount < 3)
            .OrderBy(m => m.OccurredOnUtc)
            .Take(10)
            .ToListAsync();

        foreach (var outboxMessage in messages)
        {
            IDomainEvent? domainEvent = JsonConvert.DeserializeObject<IDomainEvent>(
                outboxMessage.Content,
                new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All });

            outboxMessage.RetryCount++;

            if (domainEvent is null)
            {
                _logger.LogError(new Exception("Deserialization Error"),
                    "Couldn't deserialize message with ID: {MessageId}. It's like trying to read a book in the dark.", outboxMessage.Id);
                continue;
            }

            try
            {
                await _publisher.Publish(domainEvent);
                outboxMessage.ProcessedOnUtc = DateTime.UtcNow;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Attempt to publish message with ID: {MessageId} failed. This isn't a catastrophe yet, we'll retry. Retry count: {RetryCount}",
                    outboxMessage.Id, outboxMessage.RetryCount);
            }
        }

        await dbContext.SaveChangesAsync();
    }
}