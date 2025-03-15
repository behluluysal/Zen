using FluentValidation;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Zen.Services.Coupon.Application.Coupons;

public class UpdateCouponCommandValidator : AbstractValidator<UpdateCouponCommand>
{
    private readonly ICouponDbContext _couponDbContext;

    public UpdateCouponCommandValidator(ICouponDbContext couponDbContext)
    {
        _couponDbContext = couponDbContext;

        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("Coupon code must be provided.")
            .MaximumLength(20).WithMessage("Coupon code must not exceed 20 characters.")
            .MustAsync(CodeMustBeUniqueAsync).WithMessage("Coupon code already exists.");

        RuleFor(x => x.Discount)
            .GreaterThan(0).WithMessage("Discount must be greater than 0.");

        RuleFor(x => x.Expiration)
            .GreaterThanOrEqualTo(DateTimeOffset.UtcNow)
            .WithMessage("Expiration date must be in the future.");
    }

    private async Task<bool> CodeMustBeUniqueAsync(UpdateCouponCommand command, string code, CancellationToken cancellationToken)
    {
        return !await _couponDbContext.Coupons
            .AnyAsync(c => c.Code == code && c.Id != command.Id, cancellationToken);
    }
}
