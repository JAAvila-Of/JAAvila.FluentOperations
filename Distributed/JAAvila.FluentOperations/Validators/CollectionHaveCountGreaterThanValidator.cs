using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the collection has more than the expected number of elements.
/// </summary>
internal class CollectionHaveCountGreaterThanValidator<T>(
    PrincipalChain<IEnumerable<T>> chain,
    int expected
) : IValidator, IRuleDescriptor
{
    public static CollectionHaveCountGreaterThanValidator<T> New(
        PrincipalChain<IEnumerable<T>> chain,
        int expected
    ) => new(chain, expected);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "Collection.HaveCountGreaterThan";
    string IRuleDescriptor.OperationName => "HaveCountGreaterThan";
    Type IRuleDescriptor.SubjectType => typeof(IEnumerable<>);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object> { ["value"] = expected };

    public bool Validate()
    {
        var count = chain.GetValue().Count();

        if (count > expected)
        {
            return true;
        }

        ResultValidation =
            "The resulting collection was expected to have more than {0} element(s), but it had {1}.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}
