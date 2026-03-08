using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the string length is within the specified range.
/// </summary>
internal class StringHaveLengthBetweenValidator(PrincipalChain<string?> chain, int min, int max)
    : IValidator
{
    public static StringHaveLengthBetweenValidator New(
        PrincipalChain<string?> chain,
        int min,
        int max
    ) => new(chain, min, max);

    public string Expected { get; }
    public string ResultValidation { get; set; }

    public bool Validate()
    {
        var len = chain.GetValue()!.Length;

        if (len >= min && len <= max)
        {
            return true;
        }

        ResultValidation =
            "The value was expected to have a length between {0} and {1}, but the actual length was {2}.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}
