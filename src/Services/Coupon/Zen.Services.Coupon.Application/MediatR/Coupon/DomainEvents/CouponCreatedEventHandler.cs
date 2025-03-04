using MediatR;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using Zen.Application.Utilities.Transaction;
using Zen.Domain.Repositories;
using Zen.Domain.Utilities;
using Zen.Services.Coupon.Domain.Events;

namespace Zen.Services.Coupon.Application.MediatR.Coupon.DomainEvents;

internal sealed class CouponCreatedEventHandler(
    ILogger<CouponCreatedEventHandler> logger,
    IAuditHistoryRepository auditHistoryRepository,
    IUnitOfWork unitOfWork) : INotificationHandler<CouponCreatedEvent>
{
    public async Task Handle(CouponCreatedEvent domainEvent, CancellationToken cancellationToken)
    {
        var auditRecord = new AuditHistoryRecord
        {
            EntityId = domainEvent.Coupon.Id,
            Operation = AuditOperation.Insert,
            Timestamp = DateTime.UtcNow,
            Snapshot = JsonSerializer.Serialize(domainEvent.Coupon),
            ChangedBy = domainEvent.Coupon.CreatedBy
        };

        await auditHistoryRepository.AddAsync(auditRecord, cancellationToken);

        await unitOfWork.CommitAsync(cancellationToken);

        logger.LogInformation("Coupon Created Event for id: {Id} caught and processed successfully.", domainEvent.Coupon.Id);

    }
}