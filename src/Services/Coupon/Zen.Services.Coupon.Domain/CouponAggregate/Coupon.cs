using System.Text.Json.Serialization;
using Zen.Domain.Aggregates;
using Zen.Domain.Attributes;
using Zen.Domain.Common;

namespace Zen.Services.Coupon.Domain.CouponAggregate;

public class Coupon : AuditableAggregateRoot, IConcurrencyAware, ISoftDeletable
{
    public string Code { get; private set; }

    [Encrypted]
    public decimal Discount { get; private set; }
    public DateTimeOffset Expiration { get; private set; }
    public byte[]? RowVersion { get; private set; }


    public bool IsDeleted { get; private set; }


    public Coupon(string code, decimal discount, DateTimeOffset expiration)
    {
        Code = code;
        Discount = discount;
        Expiration = expiration;
        IsDeleted = false;
        Raise(new CouponCreatedEvent(this));
    }

    public void Update(string code, decimal discount, DateTimeOffset expiration)
    {
        Code = code;
        Discount = discount;
        Expiration = expiration;
        Raise(new CouponUpdatedEvent(this));
    }

    public void SoftDelete()
    {
        IsDeleted = true;
        Raise(new CouponDeletedEvent(this));
    }

#nullable disable

    [JsonConstructor]
    /// <summary>
    /// Used for serialization / deserialization only
    /// </summary>
    private Coupon() { }
#nullable enable
}
