namespace Zen.Application.Utilities.Validation;

/// <summary>
/// Represents the result of a validation.
/// </summary>
public class ValidationResult
{
    public bool IsValid { get; set; } = true;
    public List<string> Errors { get; set; } = [];

    public static ValidationResult Success() => new() { IsValid = true };
    public static ValidationResult Failure(IEnumerable<string> errors) =>
        new()
        { IsValid = false, Errors = new List<string>(errors) };
}
