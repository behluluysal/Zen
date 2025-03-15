using Ardalis.Result;
using MediatR;

namespace Zen.Services.Coupon.Application.Coupons;

public class UpdateCouponCommand(UpdateCouponRequest request, string rowVersion) : IRequest<Result>
{
    public string Id { get; init; } = request.Id;
    public string Code { get; init; } = request.Code;
    public decimal Discount { get; init; } = request.Discount;
    public DateTimeOffset Expiration { get; init; } = request.Expiration;
    public string RowVersion { get; init; } = rowVersion;
}