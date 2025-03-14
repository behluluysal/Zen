using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Net;
using Zen.Application.Extensions;
using Zen.Application.MediatR.Common;

namespace Zen.Services.Coupon.Application.MediatR.Coupon;

internal sealed class DeleteCouponCommandHandler(
    ICouponDbContext dbContext,
    ILogger<DeleteCouponCommandHandler> logger) : IRequestHandler<DeleteCouponCommand, ZenOperationResult>
{
    public async Task<ZenOperationResult> Handle(DeleteCouponCommand request, CancellationToken cancellationToken)
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
            coupon.SoftDelete();

            byte[] originalRowVersion = Convert.FromBase64String(request.RowVersion);
            var entry = dbContext.Coupons.Entry(coupon);
            entry.Property("RowVersion").OriginalValue = originalRowVersion;
            entry.State = EntityState.Modified;

            await dbContext.SaveChangesAsync(cancellationToken);

            logger.LogInformation("Coupon with Id {Id} soft-deleted successfully.", coupon.Id);

            return ZenOperationResult.Success();
        }
        catch (DbUpdateConcurrencyException)
        {
            return ZenOperationResult.Failure(HttpStatusCode.Conflict, "The coupon was updated by another process. Please reload and try again.");
        }
    }
}

