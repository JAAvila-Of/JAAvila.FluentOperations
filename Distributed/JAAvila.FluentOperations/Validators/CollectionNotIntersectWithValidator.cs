using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the collection has no common elements with the specified collection.
/// </summary>
internal class CollectionNotIntersectWithValidator<T>(PrincipalChain<IEnumerable<T>> chain, IEnumerable<T> other) : IValidator
{
    public static CollectionNotIntersectWithValidator<T> New(PrincipalChain<IEnumerable<T>> chain, IEnumerable<T> other) =>
        new(chain, other);

    public string Expected { get; }
    public string ResultValidation { get; set; }

    public bool Validate()
    {
        if (!chain.GetValue().Intersect(other).Any())
        {
            return true;
        }

        ResultValidation = "The resulting collection was expected to not intersect with [{0}], but it did.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}
