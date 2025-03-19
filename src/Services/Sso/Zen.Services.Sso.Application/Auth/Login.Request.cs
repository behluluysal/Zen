namespace Zen.Services.Sso.Application.Auth;

public record LoginUserRequest(
    string Email,
    string Password,
    string Audience
);