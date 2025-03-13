using Zen.Domain.Events;
using CouponRoot = Zen.Services.Coupon.Domain.Entities.CouponAggregate.Coupon;

namespace Zen.Services.Coupon.Domain.Events;

public record CouponCreatedEvent(CouponRoot Coupon) : ZenDomainEvent;
public record CouponUpdatedEvent(CouponRoot Coupon) : ZenDomainEvent;
public record CouponDeletedEvent(CouponRoot Coupon) : ZenDomainEvent;