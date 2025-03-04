namespace Zen.Application.Utilities.Context;

/// <summary>
/// Provides access to information about the current Zen user context.
/// </summary>
public interface IZenUserContext
{
    /// <summary>
    /// Gets the identifier of the currently authenticated user.
    /// </summary>
    string UserId { get; set; }
}

/// <summary>
/// Represents the current user context for Zen.
/// </summary>
public class ZenUserContext : IZenUserContext
{
    public string UserId { get; set; } = "Undefined";
}
