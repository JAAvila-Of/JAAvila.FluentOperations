using JAAvila.FluentOperations.Contract;
using JAAvila.SafeTypes.Extension;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the collection is NOT sequence-equal to the expected collection.
/// </summary>
internal class CollectionNotBeSequenceEqualToValidator<T>(
    PrincipalChain<IEnumerable<T>> chain,
    IEnumerable<T> expected
) : IValidator, IRuleDescriptor
{
    public static CollectionNotBeSequenceEqualToValidator<T> New(
        PrincipalChain<IEnumerable<T>> chain,
        IEnumerable<T> expected
    ) => new(chain, expected);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "Collection.NotBeSequenceEqualTo";
    string IRuleDescriptor.OperationName => "NotBeSequenceEqualTo";
    Type IRuleDescriptor.SubjectType => typeof(IEnumerable<>);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object> { ["values"] = expected };

    public bool Validate()
    {
        var actual = chain.GetValue();

        // If actual is null but expected is not, they are NOT sequence-equal => pass
        if (actual is null)
        {
            return true;
        }

        var actualList = actual.ToList();
        var expectedList = expected.ToList();

        if (actualList.Count != expectedList.Count)
        {
            return true;
        }

        if (
            actualList
                .Where((t, i) => !EqualityComparer<T>.Default.Equals(t, expectedList[i]))
                .Any()
        )
        {
            return true;
        }

        ResultValidation =
            "The collection was expected to not be sequence-equal, but the collections were identical in order and content.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}
