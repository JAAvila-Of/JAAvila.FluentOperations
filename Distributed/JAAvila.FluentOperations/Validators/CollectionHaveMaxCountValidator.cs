using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the collection has at most the expected number of elements.
/// </summary>
internal class CollectionHaveMaxCountValidator<T>(
    PrincipalChain<IEnumerable<T>> chain,
    int max) : IValidator
{
    public static CollectionHaveMaxCountValidator<T> New(
        PrincipalChain<IEnumerable<T>> chain,
        int max) => new(chain, max);

    public string Expected { get; }
    public string ResultValidation { get; set; }

    public bool Validate()
    {
        var count = chain.GetValue()?.Count() ?? 0;

        if (count > max)
        {
            ResultValidation = "The collection was expected to have at most {0} elements, but it had {1}.";
            return false;
        }

        return true;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}
