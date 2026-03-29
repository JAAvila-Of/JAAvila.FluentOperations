using JAAvila.FluentOperations.Contract;
using JAAvila.SafeTypes.Extension;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable long value is not one of the specified disallowed values.
/// </summary>
internal class NullableLongNotBeOneOfValidator(PrincipalChain<long?> chain, long[] values)
    : IValidator,
        IRuleDescriptor
{
    public static NullableLongNotBeOneOfValidator New(PrincipalChain<long?> chain, long[] values) =>
        new(chain, values);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "NullableLong.NotBeOneOf";
    string IRuleDescriptor.OperationName => "NotBeOneOf";
    Type IRuleDescriptor.SubjectType => typeof(long?);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object> { ["values"] = values };

    public bool Validate()
    {
        var value = chain.GetValue().SafeNull();

        if (!values.Contains(value))
        {
            return true;
        }

        ResultValidation =
            "The resulting value was expected not to be one of the given values, but it was.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}
