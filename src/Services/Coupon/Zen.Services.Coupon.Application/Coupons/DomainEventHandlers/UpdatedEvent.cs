using Ardalis.GuardClauses;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using Zen.Application.Common.Interfaces;
using Zen.Domain.Auditing;
using Zen.Services.Coupon.Domain.Events;

namespace Zen.Services.Coupon.Application.Coupons.DomainEventHandlers;

internal sealed class CouponUpdatedEventHandler(
    ILogger<CouponUpdatedEventHandler> logger,
    IAuditHistoryService auditHistoryService) : INotificationHandler<CouponUpdatedEvent>
{
    public async Task Handle(CouponUpdatedEvent domainEvent, CancellationToken cancellationToken)
    {
        var coupon = domainEvent.Coupon;

        var updatedDate = Guard.Against.Null(coupon.UpdatedDate, nameof(coupon.UpdatedDate));
        var updatedBy = Guard.Against.NullOrEmpty(coupon.UpdatedBy, nameof(coupon.UpdatedBy));

        var auditRecord = new AuditHistoryRecord
        {
            EntityId = coupon.Id,
            Operation = AuditOperation.Update,
            Timestamp = updatedDate,
            Snapshot = JsonSerializer.Serialize(coupon),
            ChangedBy = updatedBy
        };

        await auditHistoryService.LogAuditAsync(auditRecord, cancellationToken);

        logger.LogInformation("Coupon Updated Event for id: {Id} caught and processed successfully.", coupon.Id);
    }
}