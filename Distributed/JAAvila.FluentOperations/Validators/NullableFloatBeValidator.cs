using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable float value equals the expected value.
/// </summary>
internal class NullableFloatBeValidator(PrincipalChain<float?> chain, float? expected) : IValidator, IRuleDescriptor
{
    public static NullableFloatBeValidator New(PrincipalChain<float?> chain, float? expected) =>
        new(chain, expected);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "NullableFloat.Be";
    string IRuleDescriptor.OperationName => "Be";
    Type IRuleDescriptor.SubjectType => typeof(float?);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object> { ["value"] = expected };

    public bool Validate()
    {
        var value = chain.GetValue();

        if (Nullable.Equals(value, expected))
        {
            return true;
        }

        ResultValidation = "The resulting value was expected to be {0}, but {1} was found.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}
