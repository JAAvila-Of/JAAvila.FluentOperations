using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the collection does not contain all of the specified items simultaneously.
/// Passes if at least one of the expected items is missing from the collection.
/// </summary>
internal class CollectionNotContainAllValidator<T>(
    PrincipalChain<IEnumerable<T>> chain,
    params T[] expected
) : IValidator, IRuleDescriptor
{
    public static CollectionNotContainAllValidator<T> New(
        PrincipalChain<IEnumerable<T>> chain,
        params T[] expected
    ) => new(chain, expected);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "Collection.NotContainAll";
    string IRuleDescriptor.OperationName => "NotContainAll";
    Type IRuleDescriptor.SubjectType => typeof(IEnumerable<>);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object> { ["values"] = expected };

    public bool Validate()
    {
        var value = chain.GetValue();

        if (!expected.All(e => value.Contains(e)))
        {
            return true;
        }

        ResultValidation =
            "The resulting collection was expected to not contain all of [{0}], but all were found.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}
