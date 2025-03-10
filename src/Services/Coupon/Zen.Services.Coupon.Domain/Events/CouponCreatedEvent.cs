using Zen.Domain.Events;

namespace Zen.Services.Coupon.Domain.Events;

public record CouponCreatedEvent(Entities.Coupon Coupon) : ZenDomainEvent;