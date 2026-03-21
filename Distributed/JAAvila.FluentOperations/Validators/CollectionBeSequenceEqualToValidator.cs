using JAAvila.FluentOperations.Contract;
using JAAvila.SafeTypes.Extension;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the collection is sequence-equal to the expected collection.
/// </summary>
internal class CollectionBeSequenceEqualToValidator<T>(
    PrincipalChain<IEnumerable<T>> chain,
    IEnumerable<T> expected
) : IValidator, IRuleDescriptor
{
    public static CollectionBeSequenceEqualToValidator<T> New(
        PrincipalChain<IEnumerable<T>> chain,
        IEnumerable<T> expected
    ) => new(chain, expected);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "Collection.BeSequenceEqualTo";
    string IRuleDescriptor.OperationName => "BeSequenceEqualTo";
    Type IRuleDescriptor.SubjectType => typeof(IEnumerable<>);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object> { ["values"] = expected };

    public bool Validate()
    {
        var actual = chain.GetValue();

        if (actual.IsNull() && expected.IsNull())
        {
            return true;
        }

        if (actual.IsNull() || expected.IsNull())
        {
            ResultValidation = "The collection was expected to be sequence-equal, but {0}.";
            return false;
        }

        var actualList = actual.ToList();
        var expectedList = expected.ToList();

        if (actualList.Count != expectedList.Count)
        {
            ResultValidation =
                "The collection was expected to have {0} items in sequence, but found {1} items.";
            return false;
        }

        var firstMismatchIndex = -1;
        for (var i = 0; i < actualList.Count; i++)
        {
            if (!EqualityComparer<T>.Default.Equals(actualList[i], expectedList[i]))
            {
                firstMismatchIndex = i;
                break;
            }
        }

        if (firstMismatchIndex < 0)
        {
            return true;
        }

        ResultValidation =
            $"The collection differs at index {firstMismatchIndex}: expected \"{expectedList[firstMismatchIndex]}\" but found \"{actualList[firstMismatchIndex]}\".";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}
