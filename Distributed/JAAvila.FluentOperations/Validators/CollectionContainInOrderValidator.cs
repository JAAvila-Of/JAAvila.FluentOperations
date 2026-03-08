using JAAvila.FluentOperations.Contract;
using JAAvila.SafeTypes.Extension;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the collection contains the expected elements in order.
/// </summary>
internal class CollectionContainInOrderValidator<T>(
    PrincipalChain<IEnumerable<T>> chain,
    T[] expected
) : IValidator
{
    public static CollectionContainInOrderValidator<T> New(
        PrincipalChain<IEnumerable<T>> chain,
        T[] expected
    ) => new(chain, expected);

    public string Expected { get; }
    public string ResultValidation { get; set; }

    public bool Validate()
    {
        var collection = chain.GetValue();

        // An empty expected array is trivially satisfied
        if (expected.Length == 0)
        {
            return true;
        }

        if (collection.IsNull())
        {
            ResultValidation =
                "The collection was expected to contain [{0}] in the specified order, but the order constraint was not satisfied.";
            return false;
        }

        // Scan the collection finding each expected element in sequence
        var expectedIndex = 0;

        foreach (var item in collection)
        {
            if (!EqualityComparer<T>.Default.Equals(item, expected[expectedIndex]))
            {
                continue;
            }

            expectedIndex++;

            if (expectedIndex == expected.Length)
            {
                return true;
            }
        }

        ResultValidation =
            "The collection was expected to contain [{0}] in the specified order, but the order constraint was not satisfied.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}
