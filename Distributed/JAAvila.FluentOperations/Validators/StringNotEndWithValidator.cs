using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the string does not end with the expected substring.
/// </summary>
internal class StringNotEndWithValidator(string suffix, PrincipalChain<string?> chain) : IValidator
{
    public static StringNotEndWithValidator New(string suffix, PrincipalChain<string?> chain) =>
        new(suffix, chain);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "String.NotEndWith";

    public bool Validate()
    {
        if (!chain.GetValue()!.EndsWith(suffix, StringComparison.Ordinal))
        {
            return true;
        }

        ResultValidation = "The value was expected to not end with \"{0}\".";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}
