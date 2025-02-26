using MediatR;
using Zen.Application.Utilities.Common;
using Zen.Services.Coupon.Application.Dtos;

namespace Zen.Services.Coupon.Application.MediatR.Coupon;

public record CreateCouponCommand(string Code, decimal Discount, DateTime Expiration)
       : IRequest<OperationResult<CouponDto>>;


public record GetCouponByIdQuery(string CouponId) : IRequest<OperationResult<CouponDto>>;