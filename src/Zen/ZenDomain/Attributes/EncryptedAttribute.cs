namespace Zen.Domain.Attributes;

/// <summary>
/// Attribute to mark a property for encryption.
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class EncryptedAttribute : Attribute
{
}