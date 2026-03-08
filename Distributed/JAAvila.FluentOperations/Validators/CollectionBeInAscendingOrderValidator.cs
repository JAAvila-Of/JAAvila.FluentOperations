using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the elements in the collection are in ascending order.
/// </summary>
internal class CollectionBeInAscendingOrderValidator<T>(PrincipalChain<IEnumerable<T>> chain)
    : IValidator
{
    public static CollectionBeInAscendingOrderValidator<T> New(
        PrincipalChain<IEnumerable<T>> chain
    ) => new(chain);

    public string Expected { get; }
    public string ResultValidation { get; set; }

    public bool Validate()
    {
        var list = chain.GetValue().ToList();

        if (list.Count <= 1)
        {
            return true;
        }

        var comparer = Comparer<T>.Default;

        for (var i = 0; i < list.Count - 1; i++)
        {
            if (comparer.Compare(list[i], list[i + 1]) <= 0)
            {
                continue;
            }

            ResultValidation =
                "The resulting collection was expected to be in ascending order, but it was not.";
            return false;
        }

        return true;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}
