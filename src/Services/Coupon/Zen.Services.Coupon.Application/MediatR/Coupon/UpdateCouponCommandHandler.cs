using MediatR;
using Zen.Application.MediatR.Common;
using Zen.Application.Utilities.Exception;
using Zen.Application.Utilities.Transaction;
using Zen.Services.Coupon.Domain.Repositories;

namespace Zen.Services.Coupon.Application.MediatR.Coupon;

public class UpdateCouponCommandHandler(
    ICouponRepository couponRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<UpdateCouponCommand, ZenOperationResult>
{
    private readonly ICouponRepository _couponRepository = couponRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ZenOperationResult> Handle(UpdateCouponCommand request, CancellationToken cancellationToken)
    {
        var coupon = await _couponRepository.GetByIdAsync(request.Id, cancellationToken);
        if (coupon == null)
        {
            return ZenOperationResult<string>.Failure(404, "Coupon not found.");
        }

        coupon.Code = request.Code;
        coupon.Discount = request.Discount;
        coupon.Expiration = request.Expiration;

        try
        {
            byte[] originalRowVersion = Convert.FromBase64String(request.RowVersion);
            await _couponRepository.UpdateAsync(coupon, originalRowVersion, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);
        }
        catch (ZenDbUpdateConcurrencyException)
        {
            return ZenOperationResult.Failure(409, "The coupon was updated by another process. Please reload and try again.");
        }

        return ZenOperationResult.Success();
    }
}