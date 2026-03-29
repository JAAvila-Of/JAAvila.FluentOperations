using JAAvila.FluentOperations.Contract;
using JAAvila.SafeTypes.Extension;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the collection does NOT contain the same elements as the specified collection, regardless of order.
/// </summary>
internal class CollectionNotBeEquivalentToValidator<T>(
    PrincipalChain<IEnumerable<T>> chain,
    IEnumerable<T> expected
) : IValidator, IRuleDescriptor
{
    public static CollectionNotBeEquivalentToValidator<T> New(
        PrincipalChain<IEnumerable<T>> chain,
        IEnumerable<T> expected
    ) => new(chain, expected);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "Collection.NotBeEquivalentTo";
    string IRuleDescriptor.OperationName => "NotBeEquivalentTo";
    Type IRuleDescriptor.SubjectType => typeof(IEnumerable<>);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object> { ["values"] = expected };

    public bool Validate()
    {
        var actual = chain.GetValue();

        // If actual is null but expected is not, they are NOT equivalent => pass
        if (actual.IsNull())
        {
            return true;
        }

        var actualList = actual.ToList();
        var expectedList = expected.ToList();

        if (actualList.Count != expectedList.Count)
        {
            return true;
        }

        // Check if all elements in expected exist in actual (with count matching)
        var remainingActual = new List<T>(actualList);

        foreach (
            var index in expectedList.Select(
                item => remainingActual.FindIndex(a => EqualityComparer<T>.Default.Equals(a, item))
            )
        )
        {
            if (index < 0)
            {
                return true;
            }

            remainingActual.RemoveAt(index);
        }

        ResultValidation =
            "The collection was expected to not be equivalent (ignoring order), but the collections contained the same elements.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}
