using Ardalis.Result;
using MediatR;

namespace Zen.Services.Coupon.Application.Coupons;

public record CreateCouponRequest(string Code, decimal Discount, DateTimeOffset Expiration)
       : IRequest<Result<string>>;
