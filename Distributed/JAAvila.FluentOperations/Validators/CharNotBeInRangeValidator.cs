using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the char value is outside the specified inclusive range.
/// </summary>
internal class CharNotBeInRangeValidator(PrincipalChain<char> chain, char min, char max)
    : IValidator
{
    public static CharNotBeInRangeValidator New(PrincipalChain<char> chain, char min, char max) =>
        new(chain, min, max);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "Char.NotBeInRange";

    public bool Validate()
    {
        var value = chain.GetValue();

        if (value < min || value > max)
        {
            return true;
        }

        ResultValidation =
            "The resulting value was expected to not be in range [{0}, {1}], but {2} was found.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}
