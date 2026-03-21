using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the collection has the expected length (element count).
/// </summary>
internal class CollectionHaveLengthValidator<T>(
    PrincipalChain<IEnumerable<T>> chain,
    int expected
) : IValidator, IRuleDescriptor
{
    public static CollectionHaveLengthValidator<T> New(
        PrincipalChain<IEnumerable<T>> chain,
        int expected
    ) => new(chain, expected);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "Collection.HaveLength";
    string IRuleDescriptor.OperationName => "HaveLength";
    Type IRuleDescriptor.SubjectType => typeof(IEnumerable<>);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object> { ["value"] = expected };

    public bool Validate()
    {
        var count = chain.GetValue().Count();

        if (count == expected)
        {
            return true;
        }

        ResultValidation =
            "The resulting collection was expected to have length {0}, but it had {1}.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}
