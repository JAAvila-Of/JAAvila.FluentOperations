using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the collection contains duplicate elements.
/// </summary>
internal class CollectionContainDuplicatesValidator<T>(PrincipalChain<IEnumerable<T>> chain)
    : IValidator, IRuleDescriptor
{
    public static CollectionContainDuplicatesValidator<T> New(
        PrincipalChain<IEnumerable<T>> chain
    ) => new(chain);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "Collection.ContainDuplicates";
    string IRuleDescriptor.OperationName => "ContainDuplicates";
    Type IRuleDescriptor.SubjectType => typeof(IEnumerable<>);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object>();

    public bool Validate()
    {
        var list = chain.GetValue().ToList();

        if (list.Count != list.Distinct().Count())
        {
            return true;
        }

        ResultValidation =
            "The resulting collection was expected to contain duplicates, but all elements were unique.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}
