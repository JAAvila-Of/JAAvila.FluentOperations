using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable timespan value is strictly positive.
/// </summary>
internal class NullableTimeSpanBePositiveValidator(PrincipalChain<TimeSpan?> chain)
    : IValidator,
        IRuleDescriptor
{
    public static NullableTimeSpanBePositiveValidator New(PrincipalChain<TimeSpan?> chain) =>
        new(chain);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "NullableTimeSpan.BePositive";
    string IRuleDescriptor.OperationName => "BePositive";
    Type IRuleDescriptor.SubjectType => typeof(TimeSpan?);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object>();

    public bool Validate()
    {
        if (chain.GetValue()!.Value > TimeSpan.Zero)
        {
            return true;
        }

        ResultValidation = "The resulting value was expected to be positive, but {0} was found.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}
