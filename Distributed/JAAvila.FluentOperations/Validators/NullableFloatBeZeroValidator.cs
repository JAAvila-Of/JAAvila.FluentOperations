using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable float value is zero.
/// </summary>
internal class NullableFloatBeZeroValidator(PrincipalChain<float?> chain)
    : IValidator,
        IRuleDescriptor
{
    public static NullableFloatBeZeroValidator New(PrincipalChain<float?> chain) => new(chain);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "NullableFloat.BeZero";
    string IRuleDescriptor.OperationName => "BeZero";
    Type IRuleDescriptor.SubjectType => typeof(float?);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object>();

    public bool Validate()
    {
        var value = chain.GetValue();

        if (value == 0f)
        {
            return true;
        }

        ResultValidation = "The resulting value was expected to be zero, but it was not.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}
