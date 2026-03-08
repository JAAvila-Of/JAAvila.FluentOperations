using JAAvila.FluentOperations.Comparators;
using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the string value equals the expected value.
/// </summary>
internal class StringBeValidator(PrincipalChain<string?> chain, string? expectedValue) : IValidator
{
    public static StringBeValidator New(PrincipalChain<string?> chain, string? expectedValue) =>
        new(chain, expectedValue);

    /// <inheritdoc />
    public string Expected => "Be - <not null>";

    /// <inheritdoc />
    public string ResultValidation { get; set; } = string.Empty;

    public bool Validate()
    {
        var value = chain.GetValue();

        var result = StringComparator.Compare(value, expectedValue);

        if (!result.Item1)
        {
            ResultValidation = result.Item2?.ToString() ?? string.Empty;
        }

        return result.Item1;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}
