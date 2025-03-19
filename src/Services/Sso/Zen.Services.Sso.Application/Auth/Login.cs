using Ardalis.Result;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Zen.Services.Sso.Application.Services;
using Zen.Services.Sso.Domain.UserAggregate;

namespace Zen.Services.Sso.Application.Auth;

internal sealed class LoginUserCommandHandler(UserManager<AppUser> userManager, IZenTokenService tokenService)
    : IRequestHandler<LoginUserCommand, Result<AuthResult>>
{
    public async Task<Result<AuthResult>> Handle(LoginUserCommand command, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByEmailAsync(command.Email);
        if (user is null) return Result.Unauthorized("Invalid credentials.");

        if (!await userManager.CheckPasswordAsync(user, command.Password))
            return Result.Unauthorized("Invalid credentials.");

        var tokens = await tokenService.GenerateTokensAsync(user, command.Audience);
        if (!tokens.Succeeded)
            return Result.Unauthorized(tokens.ErrorMessage);

        return Result.Success(tokens.Data);
    }
}

