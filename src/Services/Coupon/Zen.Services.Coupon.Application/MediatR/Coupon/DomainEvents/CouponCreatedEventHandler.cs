using MediatR;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using Zen.Application.Common.Interfaces;
using Zen.Domain.Auditing;
using Zen.Services.Coupon.Domain.Events;

namespace Zen.Services.Coupon.Application.MediatR.Coupon.DomainEvents;

internal sealed class CouponCreatedEventHandler(
    ILogger<CouponCreatedEventHandler> logger,
    IAuditHistoryService auditHistoryService) : INotificationHandler<CouponCreatedEvent>
{
    public async Task Handle(CouponCreatedEvent domainEvent, CancellationToken cancellationToken)
    {
        var auditRecord = new AuditHistoryRecord
        {
            EntityId = domainEvent.Coupon.Id,
            Operation = AuditOperation.Insert,
            Timestamp = domainEvent.Coupon.CreatedDate,
            Snapshot = JsonSerializer.Serialize(domainEvent.Coupon),
            ChangedBy = domainEvent.Coupon.CreatedBy
        };

        await auditHistoryService.LogAuditAsync(auditRecord, cancellationToken);

        logger.LogInformation("Coupon Created Event for id: {Id} caught and processed successfully.", domainEvent.Coupon.Id);

    }
}