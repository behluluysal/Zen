namespace Zen.Services.Sso.Application.Auth;

public record CreateUserRequest(
    string UserName,
    string Email,
    string Password,
    string ConfirmPassword
);