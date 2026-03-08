using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the collection ends with the expected substring.
/// </summary>
internal class CollectionEndWithValidator<T>(PrincipalChain<IEnumerable<T>> chain, T item)
    : IValidator
{
    public static CollectionEndWithValidator<T> New(PrincipalChain<IEnumerable<T>> chain, T item) =>
        new(chain, item);

    public string Expected { get; }
    public string ResultValidation { get; set; }

    public bool Validate()
    {
        var last = chain.GetValue().Last();

        if (last!.Equals(item))
        {
            return true;
        }

        ResultValidation =
            "The resulting collection was expected to end with {0}, but it ended with {1}.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}
