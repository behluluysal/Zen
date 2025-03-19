using Ardalis.Result;
using MediatR;
using Zen.Services.Sso.Application.Services;

namespace Zen.Services.Sso.Application.Auth;

public class LoginUserCommand(LoginUserRequest request) : IRequest<Result<AuthResult>>
{
    public string Email { get; init; } = request.Email;
    public string Password { get; init; } = request.Password;
    public string Audience { get; init; } = request.Audience;
}

