using AutoMapper;
using MediatR;
using Zen.Application.MediatR.Common;
using Zen.Services.Coupon.Application.Dtos;
using Zen.Services.Coupon.Domain.Repositories;

namespace Zen.Services.Coupon.Application.MediatR.Coupon;

public class GetCouponByIdQueryHandler : IRequestHandler<GetCouponByIdQuery, ZenOperationResult<CouponDto>>
{
    private readonly ICouponRepository _couponRepository;
    private readonly IMapper _mapper;

    public GetCouponByIdQueryHandler(ICouponRepository couponRepository, IMapper mapper)
    {
        _couponRepository = couponRepository;
        _mapper = mapper;
    }

    public async Task<ZenOperationResult<CouponDto>> Handle(GetCouponByIdQuery request, CancellationToken cancellationToken)
    {
        var coupon = await _couponRepository.GetByIdAsync(request.CouponId, cancellationToken);
        if (coupon == null)
        {
            return ZenOperationResult<CouponDto>.Failure(404, "Coupon not found");
        }
        var couponDto = _mapper.Map<CouponDto>(coupon);
        return ZenOperationResult<CouponDto>.Success(couponDto);
    }
}