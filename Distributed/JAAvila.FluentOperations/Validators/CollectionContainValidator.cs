using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the collection contains the expected element.
/// </summary>
internal class CollectionContainValidator<T>(PrincipalChain<IEnumerable<T>> chain, T item)
    : IValidator,
        IRuleDescriptor
{
    public static CollectionContainValidator<T> New(PrincipalChain<IEnumerable<T>> chain, T item) =>
        new(chain, item);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "Collection.Contain";
    string IRuleDescriptor.OperationName => "Contain";
    Type IRuleDescriptor.SubjectType => typeof(IEnumerable<>);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object> { ["value"] = item! };

    public bool Validate()
    {
        if (chain.GetValue().Contains(item))
        {
            return true;
        }

        ResultValidation = "The resulting collection was expected to contain {0}, but it did not.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}
