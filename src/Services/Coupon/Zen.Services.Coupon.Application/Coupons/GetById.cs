using Ardalis.Result;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Zen.Application.Common.Extensions;

namespace Zen.Services.Coupon.Application.Coupons;

public record GetCouponByIdQuery(string CouponId) : IRequest<Result<CouponGetByIdResponse>>;

internal sealed class GetCouponByIdQueryHandler(ICouponDbContext dbContext, IMapper mapper)
    : IRequestHandler<GetCouponByIdQuery, Result<CouponGetByIdResponse>>
{
    public async Task<Result<CouponGetByIdResponse>> Handle(GetCouponByIdQuery request, CancellationToken cancellationToken)
    {
        var coupon = await dbContext.Coupons
            .ExcludeDeleted()
            .FirstOrDefaultAsync(c => c.Id == request.CouponId, cancellationToken);
        if (coupon == null)
        {
            return Result.NotFound("Coupon not found");
        }
        var couponDto = mapper.Map<CouponGetByIdResponse>(coupon);
        return Result.Success(couponDto);
    }
}