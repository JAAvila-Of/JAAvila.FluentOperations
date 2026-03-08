using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the collection has the expected element at the specified index.
/// </summary>
internal class CollectionHaveElementAtValidator<T>(
    PrincipalChain<IEnumerable<T>> chain,
    int index,
    T expected
) : IValidator
{
    public static CollectionHaveElementAtValidator<T> New(
        PrincipalChain<IEnumerable<T>> chain,
        int index,
        T expected
    ) => new(chain, index, expected);

    public string Expected { get; }
    public string ResultValidation { get; set; }

    public bool Validate()
    {
        var list = chain.GetValue().ToList();

        if (index < 0 || index >= list.Count)
        {
            ResultValidation =
                $"The collection was expected to have {expected?.ToString() ?? "null"} at index {index}, but the index was out of range.";
            return false;
        }

        if (!EqualityComparer<T>.Default.Equals(list[index], expected))
        {
            ResultValidation =
                "The collection was expected to have {0} at index {1}, but {2} was found.";
            return false;
        }

        return true;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}
