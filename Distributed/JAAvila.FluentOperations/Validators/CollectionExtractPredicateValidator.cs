using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the collection contains at least one element matching the predicate and captures all matches for extraction.
/// </summary>
internal class CollectionExtractPredicateValidator<T>(
    PrincipalChain<IEnumerable<T>> chain,
    Func<T, bool> predicate
) : IValidator, IRuleDescriptor
{
    public static CollectionExtractPredicateValidator<T> New(
        PrincipalChain<IEnumerable<T>> chain,
        Func<T, bool> predicate
    ) => new(chain, predicate);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "Collection.ExtractPredicate";
    string IRuleDescriptor.OperationName => "ExtractPredicate";
    Type IRuleDescriptor.SubjectType => typeof(IEnumerable<>);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object>();

    /// <summary>
    /// The extracted elements after successful validation.
    /// </summary>
    public IEnumerable<T>? ExtractedValues { get; private set; }

    public bool Validate()
    {
        var matches = chain.GetValue().Where(predicate).ToList();

        if (matches.Count > 0)
        {
            ExtractedValues = matches;
            return true;
        }

        ResultValidation =
            "Expected at least one element to match the predicate, but none did.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}
