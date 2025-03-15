using Ardalis.Result;
using MediatR;

namespace Zen.Services.Coupon.Application.Coupons;

public class CreateCouponCommand(CreateCouponRequest request) : IRequest<Result<string>>
{
    public string Code { get; init; } = request.Code;
    public decimal Discount { get; init; } = request.Discount;
    public DateTimeOffset Expiration { get; init; } = request.Expiration;
}