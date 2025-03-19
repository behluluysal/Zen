using Zen.Services.Sso.Domain.UserAggregate;

namespace Zen.Services.Sso.Application.Services;

public interface IZenTokenService
{
    Task<TokenResult<AuthResult>> GenerateTokensAsync(AppUser user, string audience);
    Task<TokenResult<AuthResult>> RefreshTokenAsync(string userId, string refreshToken, string tokenFamily, string audience);
    Task<TokenResult<bool>> RevokeTokensAsync(string userId);
}

public record AuthResult(string AccessToken, string RefreshToken, string TokenFamily, DateTimeOffset AccessTokenExpiresAt, DateTimeOffset RefreshTokenExpiresAt);

public class TokenResult<T>
{
    public bool Succeeded { get; private set; }
    public string? ErrorMessage { get; private set; }
    public T Data { get; private set; }

    private TokenResult(bool succeeded, T data, string? errorMessage)
    {
        Succeeded = succeeded;
        Data = data;
        ErrorMessage = errorMessage;
    }

    public static TokenResult<T> Success(T data) => new(true, data, null);
    public static TokenResult<T> Failure(string errorMessage) => new(false, default!, errorMessage);
}