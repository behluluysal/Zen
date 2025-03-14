using Ardalis.Result;
using AutoMapper;
using MediatR;

namespace Zen.Services.Coupon.Application.Coupons;

public record CreateCouponCommand(string Code, decimal Discount, DateTimeOffset Expiration)
       : IRequest<Result<string>>;

internal sealed class CreateCouponCommandHandler(ICouponDbContext dbContext,
                                  IMapper mapper) : IRequestHandler<CreateCouponCommand, Result<string>>
{
    public async Task<Result<string>> Handle(CreateCouponCommand request, CancellationToken cancellationToken)
    {
        var coupon = new Domain.Entities.CouponAggregate.Coupon(request.Code, request.Discount, request.Expiration);

        await dbContext.Coupons.AddAsync(coupon, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        var couponDto = mapper.Map<CouponGetByIdResponse>(coupon);
        return Result.Success(couponDto.Id);
    }
}