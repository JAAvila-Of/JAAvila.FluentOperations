using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the collection does not contain any element matching the specified predicate.
/// </summary>
internal class CollectionNotContainPredicateValidator<T>(PrincipalChain<IEnumerable<T>> chain, Func<T, bool> predicate) : IValidator
{
    public static CollectionNotContainPredicateValidator<T> New(PrincipalChain<IEnumerable<T>> chain, Func<T, bool> predicate) =>
        new(chain, predicate);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "Collection.NotContainPredicate";

    public bool Validate()
    {
        if (!chain.GetValue().Any(predicate))
        {
            return true;
        }

        ResultValidation = "The resulting collection was expected to not contain any element matching the predicate, but at least one was found.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}
