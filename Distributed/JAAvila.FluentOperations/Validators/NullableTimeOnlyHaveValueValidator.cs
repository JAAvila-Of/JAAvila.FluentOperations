using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable timeonly has a value (is not null).
/// </summary>
internal class NullableTimeOnlyHaveValueValidator(PrincipalChain<TimeOnly?> chain) : IValidator, IRuleDescriptor
{
    public static NullableTimeOnlyHaveValueValidator New(PrincipalChain<TimeOnly?> chain) =>
        new(chain);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "NullableTimeOnly.HaveValue";
    string IRuleDescriptor.OperationName => "HaveValue";
    Type IRuleDescriptor.SubjectType => typeof(TimeOnly?);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object>();

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
