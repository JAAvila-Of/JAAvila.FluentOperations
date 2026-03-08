using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable guid does not have a value (is null).
/// </summary>
internal class NullableGuidNotHaveValueValidator(PrincipalChain<Guid?> chain) : IValidator
{
    public static NullableGuidNotHaveValueValidator New(PrincipalChain<Guid?> chain) =>
        new(chain);

    public string Expected { get; }
    public string ResultValidation { get; set; }

    public bool Validate()
    {
        if (!chain.GetValue().HasValue)
        {
            return true;
        }

        ResultValidation = "The resulting value was expected to not have a value, but it was {0}.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}
