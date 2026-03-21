using JAAvila.FluentOperations.Contract;
using JAAvila.SafeTypes.Extension;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable integer value is one of the specified allowed values.
/// </summary>
internal class NullableIntegerBeOneOfValidator(PrincipalChain<int?> chain, int[] values)
    : IValidator, IRuleDescriptor
{
    public static NullableIntegerBeOneOfValidator New(PrincipalChain<int?> chain, int[] values) =>
        new(chain, values);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "NullableInteger.BeOneOf";
    string IRuleDescriptor.OperationName => "BeOneOf";
    Type IRuleDescriptor.SubjectType => typeof(int?);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object> { ["values"] = values };

    public bool Validate()
    {
        var value = chain.GetValue().SafeNull();

        if (values.Contains(value))
        {
            return true;
        }

        ResultValidation =
            "The resulting value was expected to be one of the given values, but it was not.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}
