using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the collection contains at least one of the expected substrings.
/// </summary>
internal class CollectionContainAnyValidator<T>(
    PrincipalChain<IEnumerable<T>> chain,
    params T[] expected
) : IValidator
{
    public static CollectionContainAnyValidator<T> New(
        PrincipalChain<IEnumerable<T>> chain,
        params T[] expected
    ) => new(chain, expected);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "Collection.ContainAny";

    public bool Validate()
    {
        var value = chain.GetValue();

        if (expected.Any(e => value.Contains(e)))
        {
            return true;
        }

        ResultValidation =
            "The resulting collection was expected to contain at least one of [{0}], but it did not.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}
