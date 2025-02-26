namespace Zen.Infrastructure.Data.Security;

/// <summary>
/// Defines a contract for computing a secure hash for a database column value.
/// </summary>
public interface IDbColumnHasher
{
    /// <summary>
    /// Computes a secure hash for the given column value.
    /// </summary>
    string ComputeHash(string columnValue);
}