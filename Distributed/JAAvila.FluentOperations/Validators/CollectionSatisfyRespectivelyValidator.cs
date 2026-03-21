using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that each element in the collection satisfies its corresponding predicate.
/// </summary>
internal class CollectionSatisfyRespectivelyValidator<T>(
    PrincipalChain<IEnumerable<T>> chain,
    Func<T, bool>[] predicates
) : IValidator
{
    public static CollectionSatisfyRespectivelyValidator<T> New(
        PrincipalChain<IEnumerable<T>> chain,
        Func<T, bool>[] predicates
    ) => new(chain, predicates);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "Collection.SatisfyRespectively";

    public bool Validate()
    {
        var collection = chain.GetValue();
        var list = collection.ToList();

        if (list.Count != predicates.Length)
        {
            ResultValidation = "The collection has {0} elements, but {1} predicates were provided.";
            return false;
        }

        if (list.Where((t, i) => !predicates[i](t)).Any())
        {
            ResultValidation =
                "The collection was expected to satisfy the respective predicates for each element, but element at index {0} did not satisfy its predicate.";
            return false;
        }

        return true;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}
