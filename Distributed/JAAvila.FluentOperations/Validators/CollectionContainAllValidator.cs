using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the collection contains all of the expected substrings.
/// </summary>
internal class CollectionContainAllValidator<T>(
    PrincipalChain<IEnumerable<T>> chain,
    params T[] expected
) : IValidator
{
    public static CollectionContainAllValidator<T> New(
        PrincipalChain<IEnumerable<T>> chain,
        params T[] expected
    ) => new(chain, expected);

    public string Expected { get; }
    public string ResultValidation { get; set; }

    public bool Validate()
    {
        var value = chain.GetValue();

        if (expected.All(e => value.Contains(e)))
        {
            return true;
        }

        ResultValidation =
            "The resulting collection was expected to contain all of [{0}], but it did not.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}
