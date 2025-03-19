using Zen.Domain.Events;

namespace Zen.Services.Coupon.Domain.CouponAggregate;

public record CouponCreatedEvent(Coupon Coupon) : ZenDomainEvent;