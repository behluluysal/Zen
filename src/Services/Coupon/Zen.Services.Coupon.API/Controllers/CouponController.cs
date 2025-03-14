using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Zen.Application.Common.Dtos;
using Zen.Services.Coupon.Application.Coupons;

namespace Zen.Services.Coupon.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[TranslateResultToActionResult]
public class CouponController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpPost]
    [ExpectedFailures(ResultStatus.Invalid)]
    public async Task<Result<string>> CreateCoupon([FromBody] CreateCouponCommand command)
    {
        return await _mediator.Send(command);
    }

    [HttpGet("{couponId}")]
    [ProducesResponseType(typeof(CouponGetByIdResponse), StatusCodes.Status200OK)]
    [ExpectedFailures(ResultStatus.NotFound)]
    public async Task<Result<CouponGetByIdResponse>> GetCoupon(string couponId)
    {
        return await _mediator.Send(new GetCouponByIdQuery(couponId));
    }

    [HttpPut("{id}")]
    [ExpectedFailures(ResultStatus.NotFound, ResultStatus.Invalid, ResultStatus.Conflict)]
    public async Task<Result> UpdateCoupon(string id, [FromBody] UpdateCouponCommand command)
    {
        if (id != command.Id)
            return Result.Invalid(new ValidationError { Identifier = "Id", ErrorMessage = "URL id does not match command id." });

        return await _mediator.Send(command);
    }

    [HttpDelete("{id}")]
    [ExpectedFailures(ResultStatus.NotFound, ResultStatus.Conflict)]
    public async Task<Result> DeleteCoupon(string id, [FromHeader(Name = "If-Match")] string rowVersion)
    {
        return await _mediator.Send(new DeleteCouponCommand(id, rowVersion));
    }

    [HttpGet("{couponId}/histories")]
    [ExpectedFailures(ResultStatus.NotFound)]
    public async Task<Result<IEnumerable<AuditHistoryRecordDto>>> GetCouponHistories(string couponId)
    {
        return await _mediator.Send(new GetCouponHistoryQuery(couponId));
    }
}