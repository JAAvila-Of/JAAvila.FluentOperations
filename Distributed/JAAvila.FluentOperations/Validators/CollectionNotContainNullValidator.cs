using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the collection contains no null elements.
/// Reports the count and indexes of null elements found.
/// </summary>
internal class CollectionNotContainNullValidator<T>(PrincipalChain<IEnumerable<T>> chain)
    : IValidator,
        IRuleDescriptor
{
    public static CollectionNotContainNullValidator<T> New(PrincipalChain<IEnumerable<T>> chain) =>
        new(chain);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "Collection.NotContainNull";
    string IRuleDescriptor.OperationName => "NotContainNull";
    Type IRuleDescriptor.SubjectType => typeof(IEnumerable<>);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object>();

    public bool Validate()
    {
        var items = chain.GetValue();
        var nullIndexes = new List<int>();
        var index = 0;

        foreach (var item in items)
        {
            if (item is null)
            {
                nullIndexes.Add(index);
            }
            index++;
        }

        if (nullIndexes.Count == 0)
        {
            return true;
        }

        var indexList = string.Join(", ", nullIndexes);
        ResultValidation =
            $"The collection was expected to contain no null elements, "
            + $"but found {nullIndexes.Count} null element(s) at index(es): {indexList}.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}
