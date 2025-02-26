using Zen.Domain.DomainEvent;

namespace Zen.Services.Coupon.Domain.Events;

public class CouponCreatedEvent(Entities.Coupon coupon) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;

    public Entities.Coupon Coupon { get; } = coupon;
}