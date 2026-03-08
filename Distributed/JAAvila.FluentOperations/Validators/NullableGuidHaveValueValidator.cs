using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable guid has a value (is not null).
/// </summary>
internal class NullableGuidHaveValueValidator(PrincipalChain<Guid?> chain) : IValidator
{
    public static NullableGuidHaveValueValidator New(PrincipalChain<Guid?> chain) =>
        new(chain);

    public string Expected { get; }
    public string ResultValidation { get; set; }

    public bool Validate()
    {
        if (chain.GetValue().HasValue)
        {
            return true;
        }

        ResultValidation = "The resulting value was expected to have a value, but it was <null>.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}
