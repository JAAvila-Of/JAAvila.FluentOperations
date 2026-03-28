using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable timespan value is strictly negative.
/// </summary>
internal class NullableTimeSpanBeNegativeValidator(PrincipalChain<TimeSpan?> chain)
    : IValidator,
        IRuleDescriptor
{
    public static NullableTimeSpanBeNegativeValidator New(PrincipalChain<TimeSpan?> chain) =>
        new(chain);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "NullableTimeSpan.BeNegative";
    string IRuleDescriptor.OperationName => "BeNegative";
    Type IRuleDescriptor.SubjectType => typeof(TimeSpan?);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object>();

    public bool Validate()
    {
        if (chain.GetValue()!.Value < TimeSpan.Zero)
        {
            return true;
        }

        ResultValidation = "The resulting value was expected to be negative, but {0} was found.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}
