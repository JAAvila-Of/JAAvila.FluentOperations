using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the double value equals the expected value.
/// </summary>
internal class DoubleBeValidator(PrincipalChain<double> chain, double expected) : IValidator
{
    public static DoubleBeValidator New(PrincipalChain<double> chain, double expected) =>
        new(chain, expected);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "Double.Be";

    public bool Validate()
    {
        if (chain.GetValue().Equals(expected))
        {
            return true;
        }

        ResultValidation = "The resulting value was expected to be {0}, but {1} was found.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}
