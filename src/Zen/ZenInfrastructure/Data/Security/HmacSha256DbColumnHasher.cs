using System.Security.Cryptography;
using System.Text;

namespace Zen.Infrastructure.Data.Security;

// <summary>
/// An HMAC-SHA256 based implementation of IDbColumnHasher.
/// Requires a secret key, making the hash keyed and more secure.
/// </summary>
public class HmacSha256DbColumnHasher : IDbColumnHasher
{
    private readonly byte[] _secretKey;

    public HmacSha256DbColumnHasher(string secretKey)
    {
        if (string.IsNullOrEmpty(secretKey))
            throw new ArgumentNullException(nameof(secretKey));
        _secretKey = Encoding.UTF8.GetBytes(secretKey);
    }

    public string ComputeHash(string columnValue)
    {
        ArgumentNullException.ThrowIfNull(columnValue);

        using var hmac = new HMACSHA256(_secretKey);
        byte[] data = Encoding.UTF8.GetBytes(columnValue);
        byte[] hashBytes = hmac.ComputeHash(data);
        return Convert.ToHexStringLower(hashBytes);
    }
}