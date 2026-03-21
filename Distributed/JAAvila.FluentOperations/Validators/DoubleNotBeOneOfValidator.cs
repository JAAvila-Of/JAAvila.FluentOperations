using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the double value is not one of the specified disallowed values.
/// </summary>
internal class DoubleNotBeOneOfValidator(PrincipalChain<double> chain, params double[] expected) : IValidator
{
    public static DoubleNotBeOneOfValidator New(PrincipalChain<double> chain, params double[] expected) =>
        new(chain, expected);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "Double.NotBeOneOf";

    public bool Validate()
    {
        if (!expected.Contains(chain.GetValue()))
        {
            return true;
        }

        ResultValidation = "The resulting value was expected to not be one of [{0}], but {1} was found.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}
