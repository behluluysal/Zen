using Ardalis.Result;
using MediatR;
using Zen.Services.Sso.Application.Services;

namespace Zen.Services.Sso.Application.Auth;

public record RefreshTokenCommand(string UserId, string RefreshToken, string TokenFamily, string Audience) 
    : IRequest<Result<AuthResult>>;
