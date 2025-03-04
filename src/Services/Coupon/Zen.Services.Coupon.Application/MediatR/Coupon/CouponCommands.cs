using MediatR;
using Zen.Application.Dtos;
using Zen.Application.MediatR.Common;
using Zen.Services.Coupon.Application.Dtos;

namespace Zen.Services.Coupon.Application.MediatR.Coupon;

public record CreateCouponCommand(string Code, decimal Discount, DateTime Expiration)
       : IRequest<ZenOperationResult<string>>;

public record UpdateCouponCommand(string Id, string Code, decimal Discount, DateTime Expiration, string RowVersion) : IRequest<ZenOperationResult>;


// Queries
public record GetCouponByIdQuery(string CouponId) : IRequest<ZenOperationResult<CouponDto>>;
public record GetCouponHistoryQuery(string CouponId) : IRequest<ZenOperationResult<IEnumerable<AuditHistoryRecordDto>>>;