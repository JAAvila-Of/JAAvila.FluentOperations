using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable double has a value (is not null).
/// </summary>
internal class NullableDoubleHaveValueValidator(PrincipalChain<double?> chain) : IValidator
{
    public static NullableDoubleHaveValueValidator New(PrincipalChain<double?> chain) => new(chain);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "NullableDouble.HaveValue";

    public bool Validate()
    {
        if (chain.GetValue().HasValue)
        {
            return true;
        }

        ResultValidation = "The subject was expected to have a value.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}
