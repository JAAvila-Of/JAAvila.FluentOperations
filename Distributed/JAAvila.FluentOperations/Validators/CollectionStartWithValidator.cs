using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the collection starts with the expected substring.
/// </summary>
internal class CollectionStartWithValidator<T>(PrincipalChain<IEnumerable<T>> chain, T item)
    : IValidator,
        IRuleDescriptor
{
    public static CollectionStartWithValidator<T> New(
        PrincipalChain<IEnumerable<T>> chain,
        T item
    ) => new(chain, item);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "Collection.StartWith";
    string IRuleDescriptor.OperationName => "StartWith";
    Type IRuleDescriptor.SubjectType => typeof(IEnumerable<>);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object> { ["value"] = item! };

    public bool Validate()
    {
        var first = chain.GetValue().First();

        if (first!.Equals(item))
        {
            return true;
        }

        ResultValidation =
            "The resulting collection was expected to start with {0}, but it started with {1}.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}
