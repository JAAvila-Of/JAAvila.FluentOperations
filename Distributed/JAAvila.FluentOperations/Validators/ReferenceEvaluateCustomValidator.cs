using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Wraps an <see cref="ICustomValidator{T}"/> into the internal <see cref="IValidator"/> contract.
/// </summary>
internal class ReferenceEvaluateCustomValidator<TSubject>(
    PrincipalChain<TSubject> chain,
    ICustomValidator<TSubject> customValidator
) : IValidator, IRuleDescriptor
{
    public static ReferenceEvaluateCustomValidator<TSubject> New(
        PrincipalChain<TSubject> chain,
        ICustomValidator<TSubject> customValidator
    ) => new(chain, customValidator);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "Custom.Evaluate";
    string IRuleDescriptor.OperationName => "Evaluate";
    Type IRuleDescriptor.SubjectType => typeof(TSubject);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object>();

    public bool Validate()
    {
        var value = chain.GetValue();

        if (customValidator.IsValid(value))
        {
            return true;
        }

        ResultValidation = customValidator.GetFailureMessage(value);
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}
