using MediatR;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using Zen.Application.Common.Interfaces;
using Zen.Domain.Auditing;
using Zen.Services.Sso.Domain.UserAggregate;

namespace Zen.Services.Sso.Application.Users.DomainEventHandlers;

internal sealed class UserCreatedEventHandler(
    ILogger<UserCreatedEventHandler> logger,
    IAuditHistoryService auditHistoryService) : INotificationHandler<UserCreatedEvent>
{
    public async Task Handle(UserCreatedEvent domainEvent, CancellationToken cancellationToken)
    {
        var auditRecord = new AuditHistoryRecord
        {
            EntityId = domainEvent.User.Id,
            Operation = AuditOperation.Insert,
            Timestamp = domainEvent.User.CreatedDate,
            Snapshot = JsonSerializer.Serialize(domainEvent.User),
            ChangedBy = domainEvent.User.CreatedBy
        };

        await auditHistoryService.LogAuditAsync(auditRecord, cancellationToken);

        logger.LogInformation("User Created Event for id: {Id} caught and processed successfully.", domainEvent.User.Id);

    }
}