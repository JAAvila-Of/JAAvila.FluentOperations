using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the collection is empty.
/// </summary>
internal class CollectionBeEmptyValidator<T>(PrincipalChain<IEnumerable<T>> chain)
    : IValidator,
        IRuleDescriptor
{
    public static CollectionBeEmptyValidator<T> New(PrincipalChain<IEnumerable<T>> chain) =>
        new(chain);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "Collection.BeEmpty";
    string IRuleDescriptor.OperationName => "BeEmpty";
    Type IRuleDescriptor.SubjectType => typeof(IEnumerable<>);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object>();

    public bool Validate()
    {
        if (!chain.GetValue().Any())
        {
            return true;
        }

        ResultValidation =
            "The resulting collection was expected to be empty, but it contained {0} element(s).";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}
