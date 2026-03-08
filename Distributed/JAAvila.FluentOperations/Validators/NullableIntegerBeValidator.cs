using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable integer value equals the expected value.
/// </summary>
internal class NullableIntegerBeValidator(PrincipalChain<int?> chain, int? expected) : IValidator
{
    public static NullableIntegerBeValidator New(PrincipalChain<int?> chain, int? expected) =>
        new(chain, expected);

    public string Expected { get; }
    public string ResultValidation { get; set; }

    public bool Validate()
    {
        if (chain.GetValue() == expected)
        {
            return true;
        }

        ResultValidation = "The resulting value was expected to be {0}, but {1} was found.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}
