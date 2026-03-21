using JAAvila.FluentOperations.Contract;
using JAAvila.SafeTypes.Extension;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the collection contains the same elements as the specified collection, regardless of order.
/// </summary>
internal class CollectionBeEquivalentToValidator<T>(
    PrincipalChain<IEnumerable<T>> chain,
    IEnumerable<T> expected
) : IValidator
{
    public static CollectionBeEquivalentToValidator<T> New(
        PrincipalChain<IEnumerable<T>> chain,
        IEnumerable<T> expected
    ) => new(chain, expected);

    public string Expected { get; } = string.Empty;
    public string ResultValidation { get; set; }
    public string MessageKey => "Collection.BeEquivalentTo";

    public bool Validate()
    {
        var actual = chain.GetValue();

        if (actual.IsNull() && expected.IsNull())
        {
            return true;
        }

        if (actual.IsNull() || expected.IsNull())
        {
            ResultValidation = "The collection was expected to be equivalent, but {0}.";
            return false;
        }

        var actualList = actual.ToList();
        var expectedList = expected.ToList();

        if (actualList.Count != expectedList.Count)
        {
            ResultValidation =
                "The collection was expected to contain {0} items (in any order), but found {1} items.";
            return false;
        }

        // Check that each element in expected exists in actual (with count matching)
        var remainingActual = new List<T>(actualList);

        foreach (
            var index in expectedList.Select(
                item => remainingActual.FindIndex(a => EqualityComparer<T>.Default.Equals(a, item))
            )
        )
        {
            if (index < 0)
            {
                ResultValidation =
                    "The collection was expected to be equivalent (ignoring order), but the element {0} was not found.";
                return false;
            }

            remainingActual.RemoveAt(index);
        }

        return true;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}
