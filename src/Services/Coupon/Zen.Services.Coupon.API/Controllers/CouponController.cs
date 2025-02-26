using MediatR;
using Microsoft.AspNetCore.Mvc;
using Zen.Application.Utilities.Common;
using Zen.Services.Coupon.Application.Dtos;
using Zen.Services.Coupon.Application.MediatR.Coupon;

namespace Zen.Services.Coupon.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CouponController : ControllerBase
{
    private readonly IMediator _mediator;

    public CouponController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult<OperationResult<CouponDto>>> CreateCoupon([FromBody] CreateCouponCommand command)
    {
        var result = await _mediator.Send(command);
        if (result.IsSuccess)
            return Ok(result);
        return BadRequest(result);
    }

    [HttpGet("{couponId}")]
    public async Task<ActionResult<OperationResult<CouponDto>>> GetCoupon(string couponId)
    {
        var query = new GetCouponByIdQuery(couponId);
        var result = await _mediator.Send(query);
        if (result.IsSuccess)
            return Ok(result);
        return NotFound(result);
    }
}