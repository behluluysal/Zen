using Ardalis.Result;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Zen.Application.Common.Extensions;

namespace Zen.Services.Coupon.Application.Coupons;

internal sealed class UpdateCouponCommandHandler(ICouponDbContext dbContext)
    : IRequestHandler<UpdateCouponCommand, Result>
{
    public async Task<Result> Handle(UpdateCouponCommand request, CancellationToken cancellationToken)
    {
        var coupon = await dbContext.Coupons
            .ExcludeDeleted()
            .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

        if (coupon == null)
        {
            return Result.NotFound("Coupon not found.");
        }

        try
        {
            coupon.Update(request.Code, request.Discount, request.Expiration);

            byte[] originalRowVersion = Convert.FromBase64String(request.RowVersion);
            var entry = dbContext.Coupons.Entry(coupon);
            entry.Property("RowVersion").OriginalValue = originalRowVersion;
            entry.State = EntityState.Modified;
            await dbContext.SaveChangesAsync(cancellationToken);

            return Result.Success();

        }
        catch (DbUpdateConcurrencyException)
        {
            // TODO, should we guard clause it or catch the exception???
            return Result.Conflict("The coupon was updated by another process. Please reload and try again.");
        }
    }
}