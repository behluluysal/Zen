using Zen.Application.Dtos;

namespace Zen.Services.Coupon.Application.Dtos;

/// <summary>
/// Data Transfer Object representing a coupon.
/// </summary>
public class CouponDto : IConcurrencyAwareDto
{
    public string Id { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public decimal Discount { get; set; }
    public DateTimeOffset Expiration { get; set; }
    public string RowVersion { get; set; } = string.Empty;
    public bool IsDeleted { get; set; }
}