using System.Text.Json.Serialization;
using Zen.Domain.Aggregates;
using Zen.Domain.Attributes;
using Zen.Domain.Common;
using Zen.Services.Coupon.Domain.Events;

namespace Zen.Services.Coupon.Domain.Entities;

public class Coupon : AggregateRoot, IIdentifiable<string>, IConcurrencyAware, IAuditable
{
    public string Id { get; private set; }
    public string Code { get; set; }

    [Encrypted]
    public decimal Discount { get; set; }
    public DateTime Expiration { get; set; }
    public byte[]? RowVersion { get; set; }

    public string CreatedBy { get; private set; }
    public DateTime CreatedDate { get; private set; }
    public string? UpdatedBy { get; private set; }
    public DateTime? UpdatedDate { get; private set; }

    public Coupon(string code, decimal discount, DateTime expiration, string createdBy, bool raiseEvent = true)
    {
        Id = Ulid.NewUlid().ToString();
        Code = code;
        Discount = discount;
        Expiration = expiration;
        CreatedBy = createdBy;
        CreatedDate = DateTime.UtcNow;

        if (raiseEvent)
        {
            Raise(new CouponCreatedEvent(this));
        }
    }

    [JsonConstructor]
    private Coupon(string id, string code, decimal discount, DateTime expiration, string createdBy, DateTime createdDate)
            : this(code, discount, expiration, createdBy, false)
    {
        Id = id;
        CreatedDate = createdDate;
    }
}
