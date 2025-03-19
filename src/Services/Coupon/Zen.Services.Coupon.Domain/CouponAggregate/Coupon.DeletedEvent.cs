using Zen.Domain.Events;

namespace Zen.Services.Coupon.Domain.CouponAggregate;

public record CouponDeletedEvent(Coupon Coupon) : ZenDomainEvent;