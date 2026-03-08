using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable double value equals the expected value.
/// </summary>
internal class NullableDoubleBeValidator(PrincipalChain<double?> chain, double? expected)
    : IValidator
{
    public static NullableDoubleBeValidator New(PrincipalChain<double?> chain, double? expected) =>
        new(chain, expected);

    public string Expected { get; }
    public string ResultValidation { get; set; }

    public bool Validate()
    {
        var value = chain.GetValue();

        if (value == expected)
        {
            return true;
        }

        ResultValidation = "The resulting value was expected to be {0}, but {1} was found.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}
