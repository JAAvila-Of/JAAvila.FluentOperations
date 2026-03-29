using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the collection has common elements with the specified collection.
/// </summary>
internal class CollectionIntersectWithValidator<T>(
    PrincipalChain<IEnumerable<T>> chain,
    IEnumerable<T> other
) : IValidator, IRuleDescriptor
{
    public static CollectionIntersectWithValidator<T> New(
        PrincipalChain<IEnumerable<T>> chain,
        IEnumerable<T> other
    ) => new(chain, other);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "Collection.IntersectWith";
    string IRuleDescriptor.OperationName => "IntersectWith";
    Type IRuleDescriptor.SubjectType => typeof(IEnumerable<>);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object> { ["values"] = other };

    public bool Validate()
    {
        if (chain.GetValue().Intersect(other).Any())
        {
            return true;
        }

        ResultValidation =
            "The resulting collection was expected to intersect with [{0}], but it did not.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}
