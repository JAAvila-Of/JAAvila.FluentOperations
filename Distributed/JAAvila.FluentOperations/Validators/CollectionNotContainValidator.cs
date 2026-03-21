using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the collection does not contain the expected element.
/// </summary>
internal class CollectionNotContainValidator<T>(PrincipalChain<IEnumerable<T>> chain, T item) : IValidator
{
    public static CollectionNotContainValidator<T> New(PrincipalChain<IEnumerable<T>> chain, T item) =>
        new(chain, item);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "Collection.NotContain";

    public bool Validate()
    {
        if (!chain.GetValue().Contains(item))
        {
            return true;
        }

        ResultValidation = "The resulting collection was expected to not contain {0}, but it did.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}
