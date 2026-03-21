using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the collection contains exactly one element matching the predicate and captures it.
/// </summary>
internal class CollectionExtractSinglePredicateValidator<T>(
    PrincipalChain<IEnumerable<T>> chain,
    Func<T, bool> predicate
) : IValidator
{
    public static CollectionExtractSinglePredicateValidator<T> New(
        PrincipalChain<IEnumerable<T>> chain,
        Func<T, bool> predicate
    ) => new(chain, predicate);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "Collection.ExtractSinglePredicate";

    /// <summary>
    /// The extracted element after successful validation.
    /// </summary>
    public T? ExtractedValue { get; private set; }

    public bool Validate()
    {
        var matches = chain.GetValue().Where(predicate).ToList();

        if (matches.Count == 1)
        {
            ExtractedValue = matches[0];
            return true;
        }

        ResultValidation =
            "The resulting collection was expected to contain exactly 1 matching element for extraction, but {0} matched.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}
