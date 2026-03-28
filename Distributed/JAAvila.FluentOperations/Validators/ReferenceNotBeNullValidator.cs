using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the reference value is not null.
/// </summary>
internal class ReferenceNotBeNullValidator<TSubject>(PrincipalChain<TSubject> chain)
    : IValidator,
        IRuleDescriptor
{
    public static ReferenceNotBeNullValidator<TSubject> New(PrincipalChain<TSubject> chain) =>
        new(chain);

    public string Expected => "Not Be Null <not null>";
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "Reference.NotBeNull";

    string IRuleDescriptor.OperationName => "NotBeNull";
    Type IRuleDescriptor.SubjectType => typeof(TSubject);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object>();

    public bool Validate()
    {
        if (chain.GetValue() != null)
        {
            return true;
        }

        ResultValidation =
            "The resulting value was expected not to be <null>, but <null> was found.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}
