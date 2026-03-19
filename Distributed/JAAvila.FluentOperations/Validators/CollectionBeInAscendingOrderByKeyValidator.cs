using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the elements in the collection are in ascending order
/// when projected through the specified key selector.
/// </summary>
internal class CollectionBeInAscendingOrderByKeyValidator<T, TKey>(
    PrincipalChain<IEnumerable<T>> chain,
    Func<T, TKey> keySelector
) : IValidator
    where TKey : IComparable<TKey>
{
    public static CollectionBeInAscendingOrderByKeyValidator<T, TKey> New(
        PrincipalChain<IEnumerable<T>> chain,
        Func<T, TKey> keySelector
    ) => new(chain, keySelector);

    public string Expected { get; }
    public string ResultValidation { get; set; }

    public bool Validate()
    {
        var list = chain.GetValue().ToList();

        if (list.Count <= 1)
        {
            return true;
        }

        var comparer = Comparer<TKey>.Default;

        for (var i = 0; i < list.Count - 1; i++)
        {
            var currentKey = keySelector(list[i]);
            var nextKey = keySelector(list[i + 1]);

            if (comparer.Compare(currentKey, nextKey) <= 0)
            {
                continue;
            }

            ResultValidation =
                $"The resulting collection was expected to be in ascending order by the selected key, "
                + $"but element at index {i} (key: '{currentKey}') was greater than element at index {i + 1} (key: '{nextKey}').";
            return false;
        }

        return true;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}
