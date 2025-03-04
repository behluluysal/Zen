using AutoMapper;
using MediatR;
using Zen.Application.MediatR.Common;
using Zen.Application.Utilities.Context;
using Zen.Application.Utilities.Transaction;
using Zen.Services.Coupon.Application.Dtos;
using Zen.Services.Coupon.Domain.Repositories;

namespace Zen.Services.Coupon.Application.MediatR.Coupon;

public class CreateCouponCommandHandler(ICouponRepository couponRepository,
                                  IUnitOfWork unitOfWork,
                                  IMapper mapper,
                                  IZenUserContext userContex) : IRequestHandler<CreateCouponCommand, ZenOperationResult<string>>
{
    private readonly ICouponRepository _couponRepository = couponRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;
    private readonly IZenUserContext _userContext = userContex;

    public async Task<ZenOperationResult<string>> Handle(CreateCouponCommand request, CancellationToken cancellationToken)
    {
        var coupon = new Domain.Entities.Coupon(request.Code, request.Discount, request.Expiration, _userContext.UserId);

        await _couponRepository.AddAsync(coupon, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);

        var couponDto = _mapper.Map<CouponDto>(coupon);
        return ZenOperationResult<string>.Success(couponDto.Id);
    }
}