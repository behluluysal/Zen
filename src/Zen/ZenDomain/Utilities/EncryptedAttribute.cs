namespace Zen.Domain.Utilities;

/// <summary>
/// Attribute to mark a property for encryption.
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class EncryptedAttribute : Attribute
{
}