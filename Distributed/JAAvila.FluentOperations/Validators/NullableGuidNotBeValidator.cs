using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable Guid value does not equal the expected value.
/// </summary>
internal class NullableGuidNotBeValidator(PrincipalChain<Guid?> chain, Guid? expected) : IValidator
{
    public static NullableGuidNotBeValidator New(PrincipalChain<Guid?> chain, Guid? expected) =>
        new(chain, expected);

    public string Expected { get; }
    public string ResultValidation { get; set; }

    public bool Validate()
    {
        if (chain.GetValue() != expected)
        {
            return true;
        }

        ResultValidation = "The resulting value was expected not to be {0}, but it was.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}
