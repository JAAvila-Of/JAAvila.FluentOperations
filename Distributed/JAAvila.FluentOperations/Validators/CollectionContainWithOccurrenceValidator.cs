using JAAvila.FluentOperations.Constraints;
using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the collection contains the expected element the specified number of times.
/// </summary>
internal class CollectionContainWithOccurrenceValidator<T>(
    PrincipalChain<IEnumerable<T>> chain,
    T item,
    OccurrenceConstraint constraint
) : IValidator, IRuleDescriptor
{
    public static CollectionContainWithOccurrenceValidator<T> New(
        PrincipalChain<IEnumerable<T>> chain,
        T item,
        OccurrenceConstraint constraint) =>
        new(chain, item, constraint);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "Collection.Contain";
    string IRuleDescriptor.OperationName => "Contain";
    Type IRuleDescriptor.SubjectType => typeof(IEnumerable<>);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object> { ["value"] = item };

    public bool Validate()
    {
        var collection = chain.GetValue();
        var comparer = EqualityComparer<T>.Default;
        var coincidences = collection.Count(x => comparer.Equals(x, item));

        if (constraint.Validate(coincidences))
        {
            return true;
        }

        ResultValidation =
            $"The collection was expected to contain {{0}} {constraint}, but it was found {(coincidences == 1 ? "1 time" : $"{coincidences} times")}.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}
