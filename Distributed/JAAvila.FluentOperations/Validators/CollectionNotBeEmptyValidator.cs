using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the collection is not empty.
/// </summary>
internal class CollectionNotBeEmptyValidator<T>(PrincipalChain<IEnumerable<T>> chain)
    : IValidator,
        IRuleDescriptor
{
    public static CollectionNotBeEmptyValidator<T> New(PrincipalChain<IEnumerable<T>> chain) =>
        new(chain);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "Collection.NotBeEmpty";
    string IRuleDescriptor.OperationName => "NotBeEmpty";
    Type IRuleDescriptor.SubjectType => typeof(IEnumerable<>);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object>();

    public bool Validate()
    {
        if (chain.GetValue().Any())
        {
            return true;
        }

        ResultValidation =
            "The resulting collection was expected to be non-empty, but it was empty.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}
