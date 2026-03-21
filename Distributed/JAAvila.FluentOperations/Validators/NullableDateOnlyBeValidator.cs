using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable DateOnly value equals the expected value.
/// </summary>
internal class NullableDateOnlyBeValidator(PrincipalChain<DateOnly?> chain, DateOnly? expected) : IValidator
{
    public static NullableDateOnlyBeValidator New(PrincipalChain<DateOnly?> chain, DateOnly? expected) =>
        new(chain, expected);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "NullableDateOnly.Be";

    public bool Validate()
    {
        if (chain.GetValue() == expected)
        {
            return true;
        }

        ResultValidation = "The resulting value was expected to be {0}, but {1} was found.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}
