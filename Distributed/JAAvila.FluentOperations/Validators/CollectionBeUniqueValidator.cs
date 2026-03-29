using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that all elements in the collection are unique.
/// </summary>
internal class CollectionBeUniqueValidator<T>(PrincipalChain<IEnumerable<T>> chain)
    : IValidator,
        IRuleDescriptor
{
    public static CollectionBeUniqueValidator<T> New(PrincipalChain<IEnumerable<T>> chain) =>
        new(chain);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "Collection.BeUnique";
    string IRuleDescriptor.OperationName => "BeUnique";
    Type IRuleDescriptor.SubjectType => typeof(IEnumerable<>);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object>();

    public bool Validate()
    {
        var list = chain.GetValue().ToList();

        if (list.Count == list.Distinct().Count())
        {
            return true;
        }

        ResultValidation =
            "The resulting collection was expected to have all unique elements, but it contained duplicates.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}
