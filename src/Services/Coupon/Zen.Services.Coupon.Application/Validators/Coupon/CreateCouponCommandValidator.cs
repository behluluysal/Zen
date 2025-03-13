using FluentValidation;
using Zen.Services.Coupon.Application.MediatR.Coupon;

namespace Zen.Services.Coupon.Application.Validators.Coupon;

public class CreateCouponCommandValidator : AbstractValidator<CreateCouponCommand>
{
    public CreateCouponCommandValidator()
    {
        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("Coupon code must be provided.")
            .MaximumLength(20).WithMessage("Coupon code must not exceed 20 characters.");

        RuleFor(x => x.Discount)
            .GreaterThan(0).WithMessage("Discount must be greater than 0.");

        RuleFor(x => x.Expiration)
            .GreaterThanOrEqualTo(DateTimeOffset.UtcNow)
            .WithMessage("Expiration date must be in the future.");
    }
}