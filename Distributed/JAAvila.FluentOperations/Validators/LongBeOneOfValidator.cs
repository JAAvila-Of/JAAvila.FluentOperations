using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the long value is one of the specified allowed values.
/// </summary>
internal class LongBeOneOfValidator(PrincipalChain<long> chain, params long[] expected) : IValidator
{
    public static LongBeOneOfValidator New(PrincipalChain<long> chain, params long[] expected) =>
        new(chain, expected);

    public string Expected { get; }
    public string ResultValidation { get; set; }

    public bool Validate()
    {
        if (expected.Contains(chain.GetValue()))
        {
            return true;
        }

        ResultValidation = "The resulting value was expected to be one of [{0}], but {1} was found.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}
