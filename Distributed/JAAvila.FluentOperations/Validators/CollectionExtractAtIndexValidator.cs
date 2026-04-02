using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the collection has an element at the specified index and captures it for extraction.
/// </summary>
internal class CollectionExtractAtIndexValidator<T>(PrincipalChain<IEnumerable<T>> chain, int index)
    : IValidator,
        IRuleDescriptor
{
    public static CollectionExtractAtIndexValidator<T> New(
        PrincipalChain<IEnumerable<T>> chain,
        int index
    ) => new(chain, index);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "Collection.ExtractAtIndex";
    string IRuleDescriptor.OperationName => "ExtractAtIndex";
    Type IRuleDescriptor.SubjectType => typeof(IEnumerable<>);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object> { ["index"] = index };

    /// <summary>
    /// The extracted element after successful validation.
    /// </summary>
    public T? ExtractedValue { get; private set; }

    public bool Validate()
    {
        var list = chain.GetValue().ToList();

        if (index >= 0 && index < list.Count)
        {
            ExtractedValue = list[index];
            return true;
        }

        ResultValidation =
            "Expected index {0} to be within range [0, {1}), but the collection has {1} elements.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}
