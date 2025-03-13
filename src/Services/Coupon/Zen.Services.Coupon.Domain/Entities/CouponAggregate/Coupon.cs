using System.Text.Json.Serialization;
using Zen.Domain.Aggregates;
using Zen.Domain.Attributes;
using Zen.Domain.Common;
using Zen.Services.Coupon.Domain.Events;

namespace Zen.Services.Coupon.Domain.Entities.CouponAggregate;

public class Coupon : AuditableAggregateRoot, IConcurrencyAware, ISoftDeletable
{
    public string Code { get; set; }

    [Encrypted]
    public decimal Discount { get; set; }
    public DateTimeOffset Expiration { get; set; }
    public byte[]? RowVersion { get; set; }


    public bool IsDeleted { get; set; }


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
