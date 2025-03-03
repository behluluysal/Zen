using Zen.Domain;
using Zen.Domain.Utilities;
using Zen.Services.Coupon.Domain.Events;

namespace Zen.Services.Coupon.Domain.Entities;

public class Coupon : AggregateRoot, IIdentifiable<string>, IConcurrencyAware
{
    public string Id { get; private set; } = Ulid.NewUlid().ToString();

    public string Code { get; set; }

    [Encrypted]
    public decimal Discount { get; set; }

    public DateTime Expiration { get; set; }

    public byte[]? RowVersion { get; set; }

    public Coupon(string code, decimal discount, DateTime expiration)
    {
        Code = code;
        Discount = discount;
        Expiration = expiration;
        Raise(new CouponCreatedEvent(this));
    }
}
