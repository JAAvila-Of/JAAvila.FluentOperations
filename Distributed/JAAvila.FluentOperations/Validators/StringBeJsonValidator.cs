using System.Text.Json;
using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the string is valid JSON.
/// </summary>
internal class StringBeJsonValidator(PrincipalChain<string?> chain) : IValidator, IRuleDescriptor
{
    public static StringBeJsonValidator New(PrincipalChain<string?> chain) => new(chain);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "String.BeJson";
    string IRuleDescriptor.OperationName => "BeJson";
    Type IRuleDescriptor.SubjectType => typeof(string);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object>();

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
