using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that at least one element in the collection satisfies the specified predicate.
/// </summary>
internal class CollectionAnySatisfyValidator<T>(
    PrincipalChain<IEnumerable<T>> chain,
    Func<T, bool> predicate
) : IValidator, IRuleDescriptor
{
    public static CollectionAnySatisfyValidator<T> New(
        PrincipalChain<IEnumerable<T>> chain,
        Func<T, bool> predicate
    ) => new(chain, predicate);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "Collection.AnySatisfy";
    string IRuleDescriptor.OperationName => "AnySatisfy";
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
            "The resulting collection was expected to have at least one element satisfying the predicate, but none did.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}
