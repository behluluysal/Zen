﻿using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Zen.Application.MediatR.Common;
using Zen.Services.Coupon.Application.Dtos;
using Zen.Application.Extensions;
using System.Net;

namespace Zen.Services.Coupon.Application.MediatR.Coupon;

internal sealed class GetCouponByIdQueryHandler(ICouponDbContext dbContext, IMapper mapper) 
    : IRequestHandler<GetCouponByIdQuery, ZenOperationResult<CouponDto>>
{
    public async Task<ZenOperationResult<CouponDto>> Handle(GetCouponByIdQuery request, CancellationToken cancellationToken)
    {
        var coupon = await dbContext.Coupons
            .ExcludeDeleted()
            .FirstOrDefaultAsync(c => c.Id == request.CouponId, cancellationToken);
        if (coupon == null)
        {
            return ZenOperationResult<CouponDto>.Failure(HttpStatusCode.NotFound, "Coupon not found");
        }
        var couponDto = mapper.Map<CouponDto>(coupon);
        return ZenOperationResult<CouponDto>.Success(couponDto);
    }
}