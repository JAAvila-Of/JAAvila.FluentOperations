using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the reference value is null.
/// </summary>
internal class ReferenceBeNullValidator<TSubject>(PrincipalChain<TSubject> chain) : IValidator
{
    public static ReferenceBeNullValidator<TSubject> New(PrincipalChain<TSubject> chain) =>
        new(chain);

    public string Expected => "Be Null <null>";
    public string ResultValidation { get; set; }

    public bool Validate()
    {
        if (chain.GetValue() == null)
        {
            return true;
        }

        ResultValidation =
            "The resulting value was expected to be <null>, but a non-null value was found.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}
