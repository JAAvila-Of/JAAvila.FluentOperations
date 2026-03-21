using JAAvila.FluentOperations.Contract;
using JAAvila.FluentOperations.Formatters;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that every element in the collection matches the specified predicate.
/// Reports the index and value of the first non-matching element.
/// </summary>
internal class CollectionOnlyContainValidator<T>(
    PrincipalChain<IEnumerable<T>> chain,
    Func<T, bool> predicate
) : IValidator, IRuleDescriptor
{
    public static CollectionOnlyContainValidator<T> New(
        PrincipalChain<IEnumerable<T>> chain,
        Func<T, bool> predicate
    ) => new(chain, predicate);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "Collection.OnlyContain";
    string IRuleDescriptor.OperationName => "OnlyContain";
    Type IRuleDescriptor.SubjectType => typeof(IEnumerable<>);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object>();

    public bool Validate()
    {
        var items = chain.GetValue();
        var index = 0;

        foreach (var item in items)
        {
            if (!predicate(item))
            {
                ResultValidation =
                    $"The collection was expected to only contain elements matching the predicate, " +
                    $"but the element at index {index} ({BaseFormatter.Format(item)}) did not match.";
                return false;
            }
            index++;
        }

        return true;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}
