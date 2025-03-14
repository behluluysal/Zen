using AutoMapper;
using MediatR;
using Zen.Application.MediatR.Common;
using Zen.Services.Coupon.Application.Dtos;

namespace Zen.Services.Coupon.Application.MediatR.Coupon;

internal sealed class CreateCouponCommandHandler(ICouponDbContext dbContext,
                                  IMapper mapper) : IRequestHandler<CreateCouponCommand, ZenOperationResult<string>>
{
    public async Task<ZenOperationResult<string>> Handle(CreateCouponCommand request, CancellationToken cancellationToken)
    {
        var coupon = new Domain.Entities.CouponAggregate.Coupon(request.Code, request.Discount, request.Expiration);

        await dbContext.Coupons.AddAsync(coupon, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        var couponDto = mapper.Map<CouponDto>(coupon);
        return ZenOperationResult<string>.Success(couponDto.Id);
    }
}