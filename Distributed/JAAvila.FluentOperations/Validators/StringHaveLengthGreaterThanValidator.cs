using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the string length is strictly greater than the expected value.
/// </summary>
internal class StringHaveLengthGreaterThanValidator(PrincipalChain<string?> chain, int expected)
    : IValidator
{
    public static StringHaveLengthGreaterThanValidator New(PrincipalChain<string?> chain, int expected) =>
        new(chain, expected);

    public string Expected { get; }
    public string ResultValidation { get; set; }

    public bool Validate()
    {
        if (chain.GetValue()!.Length > expected)
        {
            return true;
        }

        ResultValidation =
            "The value was expected to have a length greater than {0}, but the actual length was {1}.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}
