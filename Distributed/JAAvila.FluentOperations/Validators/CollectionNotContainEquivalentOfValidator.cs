using JAAvila.FluentOperations.Comparators;
using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that no element in the collection is structurally equivalent
/// to the specified expected object, using deep property-by-property comparison.
/// </summary>
/// <remarks>
/// Comparison is expected-driven: <see cref="ObjectComparator"/> iterates the properties
/// of <paramref name="expected"/> and looks them up on each collection element.
/// </remarks>
internal class CollectionNotContainEquivalentOfValidator<T, TExpected>(
    PrincipalChain<IEnumerable<T>> chain,
    TExpected expected,
    ComparisonOptions options
) : IValidator, IRuleDescriptor
{
    public static CollectionNotContainEquivalentOfValidator<T, TExpected> New(
        PrincipalChain<IEnumerable<T>> chain,
        TExpected expected,
        ComparisonOptions options
    ) => new(chain, expected, options);

    public string Expected { get; } = string.Empty;
    public string ResultValidation { get; set; }
    public string MessageKey => "Collection.NotContainEquivalentOf";
    string IRuleDescriptor.OperationName => "NotContainEquivalentOf";
    Type IRuleDescriptor.SubjectType => typeof(IEnumerable<>);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object> { ["value"] = expected };

    public bool Validate()
    {
        var collection = chain.GetValue();
        var items = collection.ToList();

        foreach (var element in items)
        {
            // Swap: iterate expected's properties, resolve against element
            var result = ObjectComparator.DeepCompare(expected, element, options);

            if (result.AreEqual)
            {
                ResultValidation =
                    "The collection was expected to NOT contain an element structurally equivalent to the specified object, but a matching element was found.";
                return false;
            }
        }

        return true;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}
