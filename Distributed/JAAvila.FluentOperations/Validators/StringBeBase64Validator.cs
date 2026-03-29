using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the string is a valid Base64-encoded value.
/// </summary>
internal class StringBeBase64Validator(PrincipalChain<string?> chain) : IValidator, IRuleDescriptor
{
    public static StringBeBase64Validator New(PrincipalChain<string?> chain) => new(chain);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "String.BeBase64";
    string IRuleDescriptor.OperationName => "BeBase64";
    Type IRuleDescriptor.SubjectType => typeof(string);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object>();

    public bool Validate()
    {
        var value = chain.GetValue();

        if (!IsValidBase64(value))
        {
            ResultValidation = "The value was expected to be a valid Base64-encoded string.";
            return false;
        }

        return true;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());

    private static bool IsValidBase64(string? value)
    {
        if (value is null)
        {
            return false;
        }

        // Base64 strings must have length divisible by 4
        if (value.Length % 4 != 0)
        {
            return false;
        }

        try
        {
            _ = Convert.FromBase64String(value);
            return true;
        }
        catch (FormatException)
        {
            return false;
        }
    }
}
