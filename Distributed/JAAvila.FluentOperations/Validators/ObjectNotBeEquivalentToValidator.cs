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
) : IValidator
{
    public static ObjectNotBeEquivalentToValidator New(
        PrincipalChain<object?> chain,
        object? expected,
        ComparisonOptions? options = null
    ) => new(chain, expected, options);

    public string Expected { get; } = string.Empty;
    public string ResultValidation { get; set; } = string.Empty;

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
