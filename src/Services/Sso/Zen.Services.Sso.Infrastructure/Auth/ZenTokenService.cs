using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Zen.Services.Sso.Application.Services;
using Zen.Services.Sso.Domain.UserAggregate;

namespace Zen.Services.Sso.Infrastructure.Auth;

internal sealed class ZenTokenService(
    UserManager<AppUser> userManager,
    IOptions<JwtSettings> jwtSettings,
    IHttpContextAccessor httpContextAccessor) : IZenTokenService
{
    private readonly UserManager<AppUser> _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
    private readonly IOptions<JwtSettings> _jwtSettings = jwtSettings ?? throw new ArgumentNullException(nameof(jwtSettings));
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));

    public async Task<TokenResult<AuthResult>> GenerateTokensAsync(AppUser user, string audience)
    {
        var accessTokenResult = await GenerateAccessTokenAsync(user, audience);
        if(!accessTokenResult.Succeeded)
        {
            return TokenResult<AuthResult>.Failure(accessTokenResult.ErrorMessage ?? "Wrong Credentials.");
        }
        var refreshToken = GenerateRefreshToken();
        var refreshExpiry = DateTimeOffset.UtcNow.AddDays(_jwtSettings.Value.RefreshTokenExpirationDays);

        var tokenFamily = Guid.NewGuid().ToString();

        var clientIp = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString() ?? "unknown";
        var userAgent = _httpContextAccessor.HttpContext?.Request.Headers.UserAgent.ToString() ?? "unknown";

        // Store token information
        await _userManager.SetAuthenticationTokenAsync(user, "ZenSso", "RefreshToken", refreshToken);
        await _userManager.SetAuthenticationTokenAsync(user, "ZenSso", "RefreshTokenExpiry", refreshExpiry.ToString("o"));
        await _userManager.SetAuthenticationTokenAsync(user, "ZenSso", "TokenFamily", tokenFamily);
        await _userManager.SetAuthenticationTokenAsync(user, "ZenSso", "TokenClientIp", clientIp);
        await _userManager.SetAuthenticationTokenAsync(user, "ZenSso", "TokenUserAgent", userAgent);

        var result = new AuthResult(accessTokenResult.Data.token, refreshToken, tokenFamily, accessTokenResult.Data.expiresAt, refreshExpiry);
        return TokenResult<AuthResult>.Success(result);
    }

    public async Task<TokenResult<AuthResult>> RefreshTokenAsync(string userId, string refreshToken, string tokenFamily, string audience)
    {
        if (string.IsNullOrEmpty(userId))
            return TokenResult<AuthResult>.Failure("User ID cannot be empty");

        if (string.IsNullOrEmpty(refreshToken))
            return TokenResult<AuthResult>.Failure("Refresh token cannot be empty");

        if (string.IsNullOrEmpty(tokenFamily))
            return TokenResult<AuthResult>.Failure("Token family cannot be empty");

        if (string.IsNullOrEmpty(audience))
            return TokenResult<AuthResult>.Failure("Audience cannot be empty");

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            return TokenResult<AuthResult>.Failure("User not found");

        var storedToken = await _userManager.GetAuthenticationTokenAsync(user, "ZenSso", "RefreshToken");
        var storedExpiry = await _userManager.GetAuthenticationTokenAsync(user, "ZenSso", "RefreshTokenExpiry");
        var storedFamily = await _userManager.GetAuthenticationTokenAsync(user, "ZenSso", "TokenFamily");

        if (storedToken != refreshToken)
            return TokenResult<AuthResult>.Failure("Invalid refresh token");

        if (DateTimeOffset.TryParse(storedExpiry, out var expiryDate) && expiryDate < DateTimeOffset.UtcNow)
            return TokenResult<AuthResult>.Failure("Refresh token expired");

        if (storedFamily != tokenFamily)
        {
            // Potential token reuse detected - revoke all tokens
            await RevokeTokensAsync(user.Id);
            return TokenResult<AuthResult>.Failure("Security violation detected. Please login again.");
        }

        var storedIp = await _userManager.GetAuthenticationTokenAsync(user, "ZenSso", "TokenClientIp");
        var storedUserAgent = await _userManager.GetAuthenticationTokenAsync(user, "ZenSso", "TokenUserAgent");
        var currentIp = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString() ?? "unknown";
        var currentUserAgent = _httpContextAccessor.HttpContext?.Request.Headers.UserAgent.ToString() ?? "unknown";

        if (storedIp != currentIp || storedUserAgent != currentUserAgent)
            return TokenResult<AuthResult>.Failure("Client environment changed");

        return await GenerateTokensAsync(user, audience);
    }

    public async Task<TokenResult<bool>> RevokeTokensAsync(string userId)
    {
        if (string.IsNullOrEmpty(userId))
            return TokenResult<bool>.Failure("User ID cannot be empty");

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            return TokenResult<bool>.Failure("User not found");

        await _userManager.RemoveAuthenticationTokenAsync(user, "ZenSso", "RefreshToken");
        await _userManager.RemoveAuthenticationTokenAsync(user, "ZenSso", "RefreshTokenExpiry");
        await _userManager.RemoveAuthenticationTokenAsync(user, "ZenSso", "TokenFamily");
        await _userManager.RemoveAuthenticationTokenAsync(user, "ZenSso", "TokenClientIp");
        await _userManager.RemoveAuthenticationTokenAsync(user, "ZenSso", "TokenUserAgent");

        return TokenResult<bool>.Success(true);
    }

    private async Task<TokenResult<(string token, DateTimeOffset expiresAt)>> GenerateAccessTokenAsync(AppUser user, string audience)
    {
        var audienceKeys = _jwtSettings.Value.Audiences;
        if (audienceKeys == null || !audienceKeys.TryGetValue(audience, out var secretKey))
            return TokenResult<(string, DateTimeOffset)>.Failure("Audience could not found.");

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id),
            new(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
        };

        var roles = await _userManager.GetRolesAsync(user);
        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var accessTokenExpiry = DateTimeOffset.UtcNow.AddMinutes(_jwtSettings.Value.AccessTokenExpirationMinutes);

        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Value.Issuer,
            audience: audience,
            claims: claims,
            notBefore: DateTimeOffset.UtcNow.DateTime,
            expires: accessTokenExpiry.DateTime,
            signingCredentials: creds
        );

        return TokenResult<(string, DateTimeOffset)>.Success((new JwtSecurityTokenHandler().WriteToken(token), accessTokenExpiry));
    }

    private static string GenerateRefreshToken()
    {
        var randomBytes = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes);
        return Convert.ToBase64String(randomBytes);
    }
}