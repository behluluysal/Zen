namespace Zen.Domain.Common;

/// <summary>
/// Represents an entity with an identifier.
/// Using a string for ULIDs.
/// </summary>
public interface IIdentifiable<T>
{
    T Id { get; }
}