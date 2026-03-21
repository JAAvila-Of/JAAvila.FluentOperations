using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the collection has at least the expected number of elements.
/// </summary>
internal class CollectionHaveMinCountValidator<T>(
    PrincipalChain<IEnumerable<T>> chain,
    int min) : IValidator, IRuleDescriptor
{
    public static CollectionHaveMinCountValidator<T> New(
        PrincipalChain<IEnumerable<T>> chain,
        int min) => new(chain, min);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "Collection.HaveMinCount";
    string IRuleDescriptor.OperationName => "HaveMinCount";
    Type IRuleDescriptor.SubjectType => typeof(IEnumerable<>);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object> { ["min"] = min };

    public bool Validate()
    {
        var count = chain.GetValue()?.Count() ?? 0;

        if (count < min)
        {
            ResultValidation = "The collection was expected to have at least {0} elements, but it had {1}.";
            return false;
        }

        return true;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}
