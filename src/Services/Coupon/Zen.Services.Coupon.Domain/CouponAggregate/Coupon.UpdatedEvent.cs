using Zen.Domain.Events;

namespace Zen.Services.Coupon.Domain.CouponAggregate;

public record CouponUpdatedEvent(Coupon Coupon) : ZenDomainEvent;