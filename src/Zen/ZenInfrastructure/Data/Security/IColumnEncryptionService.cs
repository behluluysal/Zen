namespace Zen.Infrastructure.Data.Security;

/// <summary>
/// Defines a contract for encrypting and decrypting column values.
/// </summary>
public interface IColumnEncryptionService
{
    string Encrypt(string plaintext);
    string Decrypt(string ciphertext);
}