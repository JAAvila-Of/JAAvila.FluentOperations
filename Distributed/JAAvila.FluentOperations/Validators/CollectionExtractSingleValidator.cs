using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the collection contains exactly one element and captures it for extraction.
/// </summary>
internal class CollectionExtractSingleValidator<T>(PrincipalChain<IEnumerable<T>> chain)
    : IValidator
{
    public static CollectionExtractSingleValidator<T> New(PrincipalChain<IEnumerable<T>> chain) =>
        new(chain);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "Collection.ExtractSingle";

    /// <summary>
    /// The extracted element after successful validation.
    /// </summary>
    public T? ExtractedValue { get; private set; }

    public bool Validate()
    {
        var list = chain.GetValue().ToList();

        if (list.Count == 1)
        {
            ExtractedValue = list[0];
            return true;
        }

        ResultValidation =
            "The resulting collection was expected to contain exactly 1 element for extraction, but it had {0}.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}
