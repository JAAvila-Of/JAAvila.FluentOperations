using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that all elements in the collection satisfy the specified predicate.
/// </summary>
internal class CollectionAllSatisfyValidator<T>(PrincipalChain<IEnumerable<T>> chain, Func<T, bool> predicate) : IValidator, IRuleDescriptor
{
    public static CollectionAllSatisfyValidator<T> New(PrincipalChain<IEnumerable<T>> chain, Func<T, bool> predicate) =>
        new(chain, predicate);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "Collection.AllSatisfy";
    string IRuleDescriptor.OperationName => "AllSatisfy";
    Type IRuleDescriptor.SubjectType => typeof(IEnumerable<>);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object>();

    public bool Validate()
    {
        if (chain.GetValue().All(predicate))
        {
            return true;
        }

        ResultValidation = "The resulting collection was expected to have all elements satisfy the predicate, but some did not.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}
