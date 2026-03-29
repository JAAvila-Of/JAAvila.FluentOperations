using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the collection is a subset of the specified collection.
/// </summary>
internal class CollectionBeSubsetOfValidator<T>(
    PrincipalChain<IEnumerable<T>> chain,
    IEnumerable<T> superset
) : IValidator, IRuleDescriptor
{
    public static CollectionBeSubsetOfValidator<T> New(
        PrincipalChain<IEnumerable<T>> chain,
        IEnumerable<T> superset
    ) => new(chain, superset);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "Collection.BeSubsetOf";
    string IRuleDescriptor.OperationName => "BeSubsetOf";
    Type IRuleDescriptor.SubjectType => typeof(IEnumerable<>);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object> { ["values"] = superset };

    public bool Validate()
    {
        if (chain.GetValue().All(superset.Contains))
        {
            return true;
        }

        ResultValidation =
            "The resulting collection was expected to be a subset of [{0}], but it was not.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}
