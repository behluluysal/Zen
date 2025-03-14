﻿using Ardalis.Result;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Zen.Application.Common.Extensions;

namespace Zen.Services.Coupon.Application.Coupons;

public record DeleteCouponCommand(string Id, string RowVersion) : IRequest<Result>;


internal sealed class DeleteCouponCommandHandler(
    ICouponDbContext dbContext,
    ILogger<DeleteCouponCommandHandler> logger) : IRequestHandler<DeleteCouponCommand, Result>
{
    public async Task<Result> Handle(DeleteCouponCommand request, CancellationToken cancellationToken)
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
            coupon.SoftDelete();

            byte[] originalRowVersion = Convert.FromBase64String(request.RowVersion);
            var entry = dbContext.Coupons.Entry(coupon);
            entry.Property("RowVersion").OriginalValue = originalRowVersion;
            entry.State = EntityState.Modified;

            await dbContext.SaveChangesAsync(cancellationToken);

            logger.LogInformation("Coupon with Id {Id} soft-deleted successfully.", coupon.Id);

            return Result.Success();
        }
        catch (DbUpdateConcurrencyException)
        {
            return Result.Conflict("The coupon was updated by another process. Please reload and try again.");
        }
    }
}

