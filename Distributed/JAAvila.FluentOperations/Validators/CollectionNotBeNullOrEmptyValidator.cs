using JAAvila.FluentOperations.Contract;
using JAAvila.SafeTypes.Extension;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the collection is not null or empty.
/// </summary>
internal class CollectionNotBeNullOrEmptyValidator<T>(PrincipalChain<IEnumerable<T>> chain)
    : IValidator,
        IRuleDescriptor
{
    public static CollectionNotBeNullOrEmptyValidator<T> New(
        PrincipalChain<IEnumerable<T>> chain
    ) => new(chain);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "Collection.NotBeNullOrEmpty";
    string IRuleDescriptor.OperationName => "NotBeNullOrEmpty";
    Type IRuleDescriptor.SubjectType => typeof(IEnumerable<>);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object>();

    public bool Validate()
    {
        var value = chain.GetValue();

        if (!value.IsNullOrEmpty())
        {
            return true;
        }

        ResultValidation =
            "The resulting collection was expected to be non-null and non-empty, but it was {0}.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}
