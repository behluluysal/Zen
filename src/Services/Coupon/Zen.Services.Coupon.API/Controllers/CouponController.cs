using MediatR;
using Microsoft.AspNetCore.Mvc;
using Zen.API.Extensions;
using Zen.API.Models;
using Zen.Application.Dtos;
using Zen.Services.Coupon.Application.Dtos;
using Zen.Services.Coupon.Application.MediatR.Coupon;

namespace Zen.Services.Coupon.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CouponController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpPost]
    public async Task<ActionResult<ZenApiResponse<string>>> CreateCoupon([FromBody] CreateCouponCommand command)
    {
        var operationResult = await _mediator.Send(command);
        return operationResult.ToCreatedAtAction(this, nameof(GetCoupon), new { couponId = operationResult.Data });
    }

    [HttpGet("{couponId}")]
    public async Task<ActionResult<ZenApiResponse<CouponDto>>> GetCoupon(string couponId)
    {
        var result = await _mediator.Send(new GetCouponByIdQuery(couponId));
        return result.ToOk(this);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ZenApiResponse>> UpdateCoupon(string id, [FromBody] UpdateCouponCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest("ID mismatch between URL and command.");
        }

        var result = await _mediator.Send(command);
        return result.ToNoContent(this);
    }

    [HttpGet("{couponId}/histories")]
    public async Task<ActionResult<ZenApiResponse<IEnumerable<AuditHistoryRecordDto>>>> GetCouponHistories(string couponId)
    {
        var result = await _mediator.Send(new GetCouponHistoryQuery(couponId));
        return result.ToOk(this);
    }
}