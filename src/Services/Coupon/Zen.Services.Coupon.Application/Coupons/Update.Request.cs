using Ardalis.Result;
using MediatR;

namespace Zen.Services.Coupon.Application.Coupons;

public record UpdateCouponRequest(string Id, string Code, decimal Discount, DateTimeOffset Expiration) : IRequest<Result>;