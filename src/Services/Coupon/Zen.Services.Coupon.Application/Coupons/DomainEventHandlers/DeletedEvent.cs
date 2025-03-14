using Ardalis.GuardClauses;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using Zen.Application.Common.Interfaces;
using Zen.Domain.Auditing;
using Zen.Services.Coupon.Domain.Events;

namespace Zen.Services.Coupon.Application.Coupons.DomainEventHandlers;

internal sealed class CouponDeletedEventHandler(
    ILogger<CouponDeletedEventHandler> logger,
    IAuditHistoryService auditHistoryService) : INotificationHandler<CouponDeletedEvent>
{
    public async Task Handle(CouponDeletedEvent domainEvent, CancellationToken cancellationToken)
    {
        var coupon = domainEvent.Coupon;

        var deletedDate = Guard.Against.Null(coupon.UpdatedDate, nameof(coupon.UpdatedDate));
        var deletedBy = Guard.Against.NullOrEmpty(coupon.UpdatedBy, nameof(coupon.UpdatedBy));

        var auditRecord = new AuditHistoryRecord
        {
            EntityId = coupon.Id,
            Operation = AuditOperation.Delete,
            Timestamp = deletedDate,
            Snapshot = JsonSerializer.Serialize(coupon),
            ChangedBy = deletedBy
        };

        await auditHistoryService.LogAuditAsync(auditRecord, cancellationToken);

        logger.LogInformation("Coupon Deleted Event for id: {Id} caught and processed successfully.", coupon.Id);
    }
}