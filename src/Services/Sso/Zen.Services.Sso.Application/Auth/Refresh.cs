using Ardalis.Result;
using MediatR;
using Zen.Services.Sso.Application.Services;

namespace Zen.Services.Sso.Application.Auth;

internal sealed class RefreshTokenCommandHandler(IZenTokenService tokenService)
    : IRequestHandler<RefreshTokenCommand, Result<AuthResult>>
{
    public async Task<Result<AuthResult>> Handle(RefreshTokenCommand command, CancellationToken cancellationToken)
    {
        var result = await tokenService.RefreshTokenAsync(command.UserId, command.RefreshToken, command.TokenFamily, command.Audience);
        if(!result.Succeeded)
        {
            return Result.Unauthorized(result.ErrorMessage);
        }

        return Result.Success(result.Data);
    }
}
