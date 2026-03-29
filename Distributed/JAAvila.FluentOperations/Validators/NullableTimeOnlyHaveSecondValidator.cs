using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable timeonly value has the expected second.
/// </summary>
internal class NullableTimeOnlyHaveSecondValidator(PrincipalChain<TimeOnly?> chain, int second)
    : IValidator,
        IRuleDescriptor
{
    public static NullableTimeOnlyHaveSecondValidator New(
        PrincipalChain<TimeOnly?> chain,
        int second
    ) => new(chain, second);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "NullableTimeOnly.HaveSecond";
    string IRuleDescriptor.OperationName => "HaveSecond";
    Type IRuleDescriptor.SubjectType => typeof(TimeOnly?);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object> { ["value"] = second };

    public bool Validate()
    {
        if (chain.GetValue()!.Value.Second == second)
        {
            return true;
        }

        ResultValidation =
            "The resulting value was expected to have second {0}, but {1} was found.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}
