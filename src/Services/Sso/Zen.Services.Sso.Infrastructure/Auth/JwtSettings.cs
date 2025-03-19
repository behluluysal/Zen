namespace Zen.Services.Sso.Infrastructure.Auth;

internal sealed class JwtSettings
{
    public string Issuer { get; init; } = string.Empty;
    public Dictionary<string, string> Audiences { get; init; } = new();
    public int AccessTokenExpirationMinutes { get; init; }
    public int RefreshTokenExpirationDays { get; init; }
}
