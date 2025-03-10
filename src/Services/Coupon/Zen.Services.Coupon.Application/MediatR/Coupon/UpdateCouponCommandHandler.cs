using MediatR;
using Microsoft.EntityFrameworkCore;
using Zen.Application.MediatR.Common;

namespace Zen.Services.Coupon.Application.MediatR.Coupon;

internal sealed class UpdateCouponCommandHandler(ICouponDbContext dbContext) 
    : IRequestHandler<UpdateCouponCommand, ZenOperationResult>
{
    public async Task<ZenOperationResult> Handle(UpdateCouponCommand request, CancellationToken cancellationToken)
    {
        var coupon = await dbContext.Coupons.FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);
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
            var entry = dbContext.Coupons.Entry(coupon);
            entry.Property("RowVersion").OriginalValue = originalRowVersion;
            entry.State = EntityState.Modified;
            await dbContext.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateConcurrencyException)
        {
            return ZenOperationResult.Failure(409, "The coupon was updated by another process. Please reload and try again.");
        }

        return ZenOperationResult.Success();
    }
}