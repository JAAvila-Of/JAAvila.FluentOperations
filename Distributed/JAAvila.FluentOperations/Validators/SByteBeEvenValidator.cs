using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the sbyte value is even.
/// </summary>
internal class SByteBeEvenValidator(PrincipalChain<sbyte> chain) : IValidator, IRuleDescriptor
{
    public static SByteBeEvenValidator New(PrincipalChain<sbyte> chain) =>
        new(chain);

    public string Expected { get; }
    public string ResultValidation { get; set; }
    public string MessageKey => "SByte.BeEven";
    string IRuleDescriptor.OperationName => "BeEven";
    Type IRuleDescriptor.SubjectType => typeof(sbyte);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object>();

    public bool Validate()
    {
        if (chain.GetValue() % 2 == 0)
        {
            return true;
        }

        ResultValidation = "The resulting value was expected to be even, but {0} was found.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}
