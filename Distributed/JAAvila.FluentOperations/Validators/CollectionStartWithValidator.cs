using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the collection starts with the expected substring.
/// </summary>
internal class CollectionStartWithValidator<T>(PrincipalChain<IEnumerable<T>> chain, T item)
    : IValidator
{
    public static CollectionStartWithValidator<T> New(
        PrincipalChain<IEnumerable<T>> chain,
        T item
    ) => new(chain, item);

    public string Expected { get; }
    public string ResultValidation { get; set; }

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
