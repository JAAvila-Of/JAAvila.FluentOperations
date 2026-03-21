using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the collection is not a subset of the specified collection.
/// </summary>
internal class CollectionNotBeSubsetOfValidator<T>(
    PrincipalChain<IEnumerable<T>> chain,
    IEnumerable<T> superset
) : IValidator, IRuleDescriptor
{
    public static CollectionNotBeSubsetOfValidator<T> New(
        PrincipalChain<IEnumerable<T>> chain,
        IEnumerable<T> superset
    ) => new(chain, superset);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "Collection.NotBeSubsetOf";
    string IRuleDescriptor.OperationName => "NotBeSubsetOf";
    Type IRuleDescriptor.SubjectType => typeof(IEnumerable<>);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object> { ["values"] = superset };

    public bool Validate()
    {
        var supersetList = superset.ToList();

        if (!chain.GetValue().All(e => supersetList.Contains(e)))
        {
            return true;
        }

        ResultValidation =
            "The resulting collection was expected to not be a subset of [{0}], but it was.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}
