using System.Text.Json;
using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the string is valid JSON.
/// </summary>
internal class StringBeJsonValidator(PrincipalChain<string?> chain) : IValidator
{
    public static StringBeJsonValidator New(PrincipalChain<string?> chain) => new(chain);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "String.BeJson";

    public bool Validate()
    {
        try
        {
            using var doc = JsonDocument.Parse(chain.GetValue()!);
            return true;
        }
        catch (JsonException)
        {
            ResultValidation = "The value was expected to be valid JSON.";
            return false;
        }
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}
