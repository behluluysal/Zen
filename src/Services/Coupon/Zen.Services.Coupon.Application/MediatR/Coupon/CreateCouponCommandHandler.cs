using AutoMapper;
using MediatR;
using Zen.Application.Common.Interfaces;
using Zen.Application.MediatR.Common;
using Zen.Services.Coupon.Application.Dtos;

namespace Zen.Services.Coupon.Application.MediatR.Coupon;

internal sealed class CreateCouponCommandHandler(ICouponDbContext dbContext,
                                  IMapper mapper,
                                  IZenUserContext userContext) : IRequestHandler<CreateCouponCommand, ZenOperationResult<string>>
{
    public async Task<ZenOperationResult<string>> Handle(CreateCouponCommand request, CancellationToken cancellationToken)
    {
        var coupon = new Domain.Entities.Coupon(request.Code, request.Discount, request.Expiration, userContext.UserId);

        await dbContext.Coupons.AddAsync(coupon, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        var couponDto = mapper.Map<CouponDto>(coupon);
        return ZenOperationResult<string>.Success(couponDto.Id);
    }
}