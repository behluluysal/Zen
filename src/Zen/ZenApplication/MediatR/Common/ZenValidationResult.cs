namespace Zen.Application.MediatR.Common;

/// <summary>
/// Represents the result of a validation.
/// </summary>
public class ZenValidationResult
{
    public bool IsValid { get; set; } = true;
    public List<string> Errors { get; set; } = [];

    public static ZenValidationResult Success() => new() { IsValid = true };
    public static ZenValidationResult Failure(IEnumerable<string> errors) =>
        new()
        { IsValid = false, Errors = new List<string>(errors) };
}