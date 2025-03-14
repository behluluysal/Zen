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
    [ProducesResponseType(typeof(ZenApiResponse<string>), StatusCodes.Status201Created)]
    public async Task<ActionResult<ZenApiResponse<string>>> CreateCoupon([FromBody] CreateCouponCommand command)
    {
        var operationResult = await _mediator.Send(command);
        return operationResult.ToCreatedAtAction(this, nameof(GetCoupon), new { couponId = operationResult.Data });
    }

    [HttpGet("{couponId}")]
    [ProducesResponseType(typeof(ZenApiResponse<CouponDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ZenApiResponse<CouponDto>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ZenApiResponse<CouponDto>>> GetCoupon(string couponId)
    {
        var result = await _mediator.Send(new GetCouponByIdQuery(couponId));
        return result.ToOk(this);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ZenApiResponse), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ZenApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ZenApiResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ZenApiResponse), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ZenApiResponse>> UpdateCoupon(string id, [FromBody] UpdateCouponCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest("ID mismatch between URL and command.");
        }

        var result = await _mediator.Send(command);
        return result.ToNoContent(this);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ZenApiResponse), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ZenApiResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ZenApiResponse), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ZenApiResponse>> DeleteCoupon(string id, [FromHeader(Name = "If-Match")] string rowVersion)
    {
        var command = new DeleteCouponCommand(id, rowVersion);
        var result = await _mediator.Send(command);

        return result.ToNoContent(this);
    }

    [HttpGet("{couponId}/histories")]
    [ProducesResponseType(typeof(ZenApiResponse<IEnumerable<AuditHistoryRecordDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ZenApiResponse<IEnumerable<AuditHistoryRecordDto>>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ZenApiResponse<IEnumerable<AuditHistoryRecordDto>>>> GetCouponHistories(string couponId)
    {
        var result = await _mediator.Send(new GetCouponHistoryQuery(couponId));
        return result.ToOk(this);
    }
}