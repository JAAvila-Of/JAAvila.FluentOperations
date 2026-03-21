using JAAvila.FluentOperations.Comparators;
using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the object contains the same elements as the specified collection, regardless of order.
/// </summary>
internal class ObjectBeEquivalentToValidator(
    PrincipalChain<object?> chain,
    object? expected,
    ComparisonOptions? options = null
) : IValidator, IRuleDescriptor
{
    public static ObjectBeEquivalentToValidator New(
        PrincipalChain<object?> chain,
        object? expected,
        ComparisonOptions? options = null
    ) => new(chain, expected, options);

    public string Expected { get; } = string.Empty;
    public string ResultValidation { get; set; }
    public string MessageKey => "Object.BeEquivalentTo";
    string IRuleDescriptor.OperationName => "BeEquivalentTo";
    Type IRuleDescriptor.SubjectType => typeof(object);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object> { ["value"] = expected };

    public bool Validate()
    {
        var actual = chain.GetValue();
        var result = ObjectComparator.DeepCompare(
            actual,
            expected,
            options ?? ComparisonOptions.Default
        );

        if (result.AreEqual)
        {
            return true;
        }

        ResultValidation =
            "The object was expected to be equivalent to the expected value, but differences were found:\n{0}";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}
