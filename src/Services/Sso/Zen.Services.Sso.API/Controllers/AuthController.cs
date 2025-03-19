using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Zen.Services.Sso.Application.Auth;
using Zen.Services.Sso.Application.Services;

namespace Zen.Services.Sso.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[TranslateResultToActionResult]
public class AuthController(IMediator mediator) : ControllerBase
{

    [HttpPost("register")]
    [ExpectedFailures(ResultStatus.Invalid)]
    public async Task<Result<string>> Register([FromBody] CreateUserRequest request)
    {
        return await mediator.Send(new RegisterUserCommand(request));
    }

    [HttpPost("login")]
    [ExpectedFailures(ResultStatus.Invalid)]
    public async Task<ActionResult<AuthResult>> Login([FromBody] LoginUserRequest request)
    {
        var result = await mediator.Send(new LoginUserCommand(request));
        return result.ToActionResult(this);
    }

    [HttpPost("refresh")]
    [ExpectedFailures(ResultStatus.Invalid)]
    public async Task<ActionResult<AuthResult>> Refresh([FromBody] RefreshTokenCommand request)
    {
        var result = await mediator.Send(request);
        return result.ToActionResult(this);
    }

}
