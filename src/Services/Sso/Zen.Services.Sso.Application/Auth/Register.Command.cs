using Ardalis.Result;
using MediatR;

namespace Zen.Services.Sso.Application.Auth;

public class RegisterUserCommand(CreateUserRequest request) : IRequest<Result<string>>
{
    public string UserName { get; init; } = request.UserName;
    public string Email { get; init; } = request.Email;
    public string Password { get; init; } = request.Password;
    public string ConfirmPassword { get; init; } = request.ConfirmPassword;
}
