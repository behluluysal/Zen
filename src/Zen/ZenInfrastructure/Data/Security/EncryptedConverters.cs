using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Globalization;

namespace Zen.Infrastructure.Data.Security;

/// <summary>
/// Value converter that converts a decimal to an invariant string, encrypts it,
/// and decrypts it back and converts to decimal when reading.
/// </summary>
internal class EncryptedDecimalConverter(IColumnEncryptionService encryptionService) : ValueConverter<decimal, string>(
          d => encryptionService.Encrypt(d.ToString(CultureInfo.InvariantCulture)),
          s => decimal.Parse(encryptionService.Decrypt(s), CultureInfo.InvariantCulture))
{
}

/// <summary>
/// Value converter that encrypts a string when saving to the database
/// and decrypts it when reading.
/// </summary>
internal class EncryptedStringConverter(IColumnEncryptionService encryptionService) : ValueConverter<string, string>(
          plaintext => encryptionService.Encrypt(plaintext),
          ciphertext => encryptionService.Decrypt(ciphertext))
{
}