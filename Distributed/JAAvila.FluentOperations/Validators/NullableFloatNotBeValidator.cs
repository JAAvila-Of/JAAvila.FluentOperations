using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable float value does not equal the expected value.
/// </summary>
internal class NullableFloatNotBeValidator(PrincipalChain<float?> chain, float? expected)
    : IValidator
{
    public static NullableFloatNotBeValidator New(PrincipalChain<float?> chain, float? expected) =>
        new(chain, expected);

    public string Expected { get; }
    public string ResultValidation { get; set; }

    public bool Validate()
    {
        if (!Nullable.Equals(chain.GetValue(), expected))
        {
            return true;
        }
        ResultValidation = "The resulting value was expected not to be {0}, but it was.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}
