using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the collection element count is within the specified inclusive range [min, max].
/// </summary>
internal class CollectionHaveCountBetweenValidator<T>(
    PrincipalChain<IEnumerable<T>> chain,
    int min,
    int max
) : IValidator, IRuleDescriptor
{
    public static CollectionHaveCountBetweenValidator<T> New(
        PrincipalChain<IEnumerable<T>> chain,
        int min,
        int max
    ) => new(chain, min, max);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "Collection.HaveCountBetween";
    string IRuleDescriptor.OperationName => "HaveCountBetween";
    Type IRuleDescriptor.SubjectType => typeof(IEnumerable<>);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object> { ["min"] = min, ["max"] = max };

    public bool Validate()
    {
        var count = chain.GetValue().Count();

        if (count >= min && count <= max)
        {
            return true;
        }

        ResultValidation =
            "The resulting collection was expected to have between {0} and {1} element(s), but it had {2}.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}
