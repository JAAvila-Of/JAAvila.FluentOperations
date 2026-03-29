using JAAvila.FluentOperations.Comparators;
using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that at least one element in the collection is structurally equivalent
/// to the specified expected object, using deep property-by-property comparison.
/// </summary>
/// <remarks>
/// Comparison is expected-driven: <see cref="ObjectComparator"/> iterates the properties
/// of <paramref name="expected"/> and looks them up on each collection element.
/// This enables partial matching with anonymous types or DTOs with fewer properties.
/// </remarks>
internal class CollectionContainEquivalentOfValidator<T, TExpected>(
    PrincipalChain<IEnumerable<T>> chain,
    TExpected expected,
    ComparisonOptions options
) : IValidator, IRuleDescriptor
{
    public static CollectionContainEquivalentOfValidator<T, TExpected> New(
        PrincipalChain<IEnumerable<T>> chain,
        TExpected expected,
        ComparisonOptions options
    ) => new(chain, expected, options);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "Collection.ContainEquivalentOf";
    string IRuleDescriptor.OperationName => "ContainEquivalentOf";
    Type IRuleDescriptor.SubjectType => typeof(IEnumerable<>);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object> { ["value"] = expected! };

    public bool Validate()
    {
        var collection = chain.GetValue();
        var items = collection.ToList();

        if (
            items
                .Select(element => ObjectComparator.DeepCompare(expected, element, options))
                .Any(result => result.AreEqual)
        )
        {
            return true;
        }

        ResultValidation =
            "The collection was expected to contain an element structurally equivalent to the specified object, but no matching element was found.\n{0}";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}
