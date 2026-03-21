using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the collection has fewer than the expected number of elements.
/// </summary>
internal class CollectionHaveCountLessThanValidator<T>(
    PrincipalChain<IEnumerable<T>> chain,
    int expected
) : IValidator, IRuleDescriptor
{
    public static CollectionHaveCountLessThanValidator<T> New(
        PrincipalChain<IEnumerable<T>> chain,
        int expected
    ) => new(chain, expected);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "Collection.HaveCountLessThan";
    string IRuleDescriptor.OperationName => "HaveCountLessThan";
    Type IRuleDescriptor.SubjectType => typeof(IEnumerable<>);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object> { ["value"] = expected };

    public bool Validate()
    {
        var count = chain.GetValue().Count();

        if (count < expected)
        {
            return true;
        }

        ResultValidation =
            "The resulting collection was expected to have fewer than {0} element(s), but it had {1}.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}
