using MediatR;
using Microsoft.Extensions.Logging;
using Zen.Services.Coupon.Domain.Events;

namespace Zen.Services.Coupon.Application.MediatR.Coupon.DomainEvents;

internal class CouponCreatedEventHandler(
    ILogger<CouponCreatedEventHandler> logger) : INotificationHandler<CouponCreatedEvent>
{


    private readonly ILogger<CouponCreatedEventHandler> _logger = logger;

    public Task Handle(CouponCreatedEvent domainEvent, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Coupon Created Event for id: {Id} caught and processed successfully.", domainEvent.Coupon.Id);
        return Task.CompletedTask;
    }
}
