using AutoMapper;
using MediatR;
using Zen.Application.Utilities.Common;
using Zen.Services.Coupon.Application.Dtos;
using Zen.Services.Coupon.Domain.Repositories;

namespace Zen.Services.Coupon.Application.MediatR.Coupon;

public class GetCouponByIdQueryHandler : IRequestHandler<GetCouponByIdQuery, OperationResult<CouponDto>>
{
    private readonly ICouponRepository _couponRepository;
    private readonly IMapper _mapper;

    public GetCouponByIdQueryHandler(ICouponRepository couponRepository, IMapper mapper)
    {
        _couponRepository = couponRepository;
        _mapper = mapper;
    }

    public async Task<OperationResult<CouponDto>> Handle(GetCouponByIdQuery request, CancellationToken cancellationToken)
    {
        var coupon = await _couponRepository.GetByIdAsync(request.CouponId, cancellationToken);
        if (coupon == null)
        {
            return OperationResult<CouponDto>.Failure(404, "Coupon not found");
        }
        var couponDto = _mapper.Map<CouponDto>(coupon);
        return OperationResult<CouponDto>.Success(couponDto);
    }
}