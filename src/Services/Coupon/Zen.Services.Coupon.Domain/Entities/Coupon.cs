using Zen.Domain;
using Zen.Domain.Utilities;

namespace Zen.Services.Coupon.Domain.Entities;

public class Coupon(string code, decimal discount, DateTime expiration) : IIdentifiable<string>, IAggregateRoot, IConcurrencyAware
{
    public string Id { get; private set; } = Ulid.NewUlid().ToString();

    public string Code { get; set; } = code;

    [Encrypted]
    public decimal Discount { get; set; } = discount;

    public DateTime Expiration { get; set; } = expiration;

    public byte[]? RowVersion { get; set; }
}
