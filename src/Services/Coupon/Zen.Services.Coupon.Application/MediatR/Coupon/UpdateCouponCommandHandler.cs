using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Net;
using Zen.Application.Common.Interfaces;
using Zen.Application.Extensions;
using Zen.Application.MediatR.Common;

namespace Zen.Services.Coupon.Application.MediatR.Coupon;

internal sealed class UpdateCouponCommandHandler(ICouponDbContext dbContext) 
    : IRequestHandler<UpdateCouponCommand, ZenOperationResult>
{
    public async Task<ZenOperationResult> Handle(UpdateCouponCommand request, CancellationToken cancellationToken)
    {
        var coupon = await dbContext.Coupons
            .ExcludeDeleted()
            .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

        if (coupon == null)
        {
            return ZenOperationResult<string>.Failure(HttpStatusCode.NotFound, "Coupon not found.");
        }

        try
        {
            coupon.Update(request.Code, request.Discount, request.Expiration);

            byte[] originalRowVersion = Convert.FromBase64String(request.RowVersion);
            var entry = dbContext.Coupons.Entry(coupon);
            entry.Property("RowVersion").OriginalValue = originalRowVersion;
            entry.State = EntityState.Modified;
            await dbContext.SaveChangesAsync(cancellationToken);

            return ZenOperationResult.Success();

        }
        catch (DbUpdateConcurrencyException)
        {
            return ZenOperationResult.Failure(HttpStatusCode.Conflict, "The coupon was updated by another process. Please reload and try again.");
        }
    }
}