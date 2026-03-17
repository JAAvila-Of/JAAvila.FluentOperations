using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the collection contains exactly one element matching the specified predicate.
/// </summary>
internal class CollectionContainSinglePredicateValidator<T>(PrincipalChain<IEnumerable<T>> chain, Func<T, bool> predicate) : IValidator
{
    public static CollectionContainSinglePredicateValidator<T> New(PrincipalChain<IEnumerable<T>> chain, Func<T, bool> predicate) =>
        new(chain, predicate);

    public string Expected { get; }
    public string ResultValidation { get; set; }

    public bool Validate()
    {
        var count = chain.GetValue().Count(predicate);

        if (count == 1)
        {
            return true;
        }

        ResultValidation = count == 0
            ? "The resulting collection was expected to contain a single element matching the predicate, but no matching element was found."
            : "The resulting collection was expected to contain a single element matching the predicate, but {0} matching elements were found.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}
