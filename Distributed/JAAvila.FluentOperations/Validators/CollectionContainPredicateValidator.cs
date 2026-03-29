using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the collection contains at least one element matching the specified predicate.
/// </summary>
internal class CollectionContainPredicateValidator<T>(
    PrincipalChain<IEnumerable<T>> chain,
    Func<T, bool> predicate
) : IValidator, IRuleDescriptor
{
    public static CollectionContainPredicateValidator<T> New(
        PrincipalChain<IEnumerable<T>> chain,
        Func<T, bool> predicate
    ) => new(chain, predicate);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "Collection.ContainPredicate";
    string IRuleDescriptor.OperationName => "ContainPredicate";
    Type IRuleDescriptor.SubjectType => typeof(IEnumerable<>);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object>();

    public bool Validate()
    {
        if (chain.GetValue().Any(predicate))
        {
            return true;
        }

        ResultValidation =
            "The resulting collection was expected to contain an element matching the predicate, but none was found.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}
