using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the collection does not contain any of the specified items.
/// Fails if even one of the expected items is found in the collection.
/// </summary>
internal class CollectionNotContainAnyValidator<T>(
    PrincipalChain<IEnumerable<T>> chain,
    params T[] expected
) : IValidator, IRuleDescriptor
{
    public static CollectionNotContainAnyValidator<T> New(
        PrincipalChain<IEnumerable<T>> chain,
        params T[] expected
    ) => new(chain, expected);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "Collection.NotContainAny";
    string IRuleDescriptor.OperationName => "NotContainAny";
    Type IRuleDescriptor.SubjectType => typeof(IEnumerable<>);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object> { ["values"] = expected };

    public bool Validate()
    {
        var value = chain.GetValue();

        if (!expected.Any(e => value.Contains(e)))
        {
            return true;
        }

        ResultValidation =
            "The resulting collection was expected to not contain any of [{0}], but at least one was found.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}
