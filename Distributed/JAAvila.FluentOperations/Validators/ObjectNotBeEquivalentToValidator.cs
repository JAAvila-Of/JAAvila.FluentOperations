using JAAvila.FluentOperations.Comparators;
using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the object is not structurally equivalent to the expected object.
/// </summary>
internal class ObjectNotBeEquivalentToValidator(
    PrincipalChain<object?> chain,
    object? expected,
    ComparisonOptions? options = null
) : IValidator, IRuleDescriptor
{
    public static ObjectNotBeEquivalentToValidator New(
        PrincipalChain<object?> chain,
        object? expected,
        ComparisonOptions? options = null
    ) => new(chain, expected, options);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "Object.NotBeEquivalentTo";
    string IRuleDescriptor.OperationName => "NotBeEquivalentTo";
    Type IRuleDescriptor.SubjectType => typeof(object);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object> { ["value"] = expected! };

    public bool Validate()
    {
        var actual = chain.GetValue();
        var result = ObjectComparator.DeepCompare(
            actual,
            expected,
            options ?? ComparisonOptions.Default
        );

        if (!result.AreEqual)
        {
            return true;
        }

        ResultValidation =
            "The object was expected to NOT be equivalent to the expected value, but they are structurally equal.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}
