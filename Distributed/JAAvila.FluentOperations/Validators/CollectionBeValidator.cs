using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the collection reference equals the expected reference (reference equality).
/// </summary>
internal class CollectionBeValidator<T>(
    PrincipalChain<IEnumerable<T>> chain,
    IEnumerable<T> expected
) : IValidator, IRuleDescriptor
{
    public static CollectionBeValidator<T> New(
        PrincipalChain<IEnumerable<T>> chain,
        IEnumerable<T> expected
    ) => new(chain, expected);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "Collection.Be";
    string IRuleDescriptor.OperationName => "Be";
    Type IRuleDescriptor.SubjectType => typeof(IEnumerable<>);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object> { ["values"] = expected };

    public bool Validate()
    {
        var value = chain.GetValue();

        if (ReferenceEquals(value, expected))
        {
            return true;
        }

        ResultValidation =
            "The resulting collection was expected to be the same reference as {0}, but a different reference was found.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}
