using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable short does not have a value (is null).
/// </summary>
internal class NullableShortNotHaveValueValidator(PrincipalChain<short?> chain) : IValidator
{
    public static NullableShortNotHaveValueValidator New(PrincipalChain<short?> chain) =>
        new(chain);

    public string Expected { get; }
    public string ResultValidation { get; set; }

    public bool Validate()
    {
        if (!chain.GetValue().HasValue)
        {
            return true;
        }

        ResultValidation = "The subject was expected not to have a value.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}
