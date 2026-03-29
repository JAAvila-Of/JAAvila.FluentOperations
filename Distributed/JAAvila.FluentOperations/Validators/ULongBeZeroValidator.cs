using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the ulong value is zero.
/// </summary>
internal class ULongBeZeroValidator(PrincipalChain<ulong> chain) : IValidator, IRuleDescriptor
{
    public static ULongBeZeroValidator New(PrincipalChain<ulong> chain) => new(chain);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "ULong.BeZero";
    string IRuleDescriptor.OperationName => "BeZero";
    Type IRuleDescriptor.SubjectType => typeof(ulong);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object>();

    public bool Validate()
    {
        if (chain.GetValue() == 0UL)
        {
            return true;
        }

        ResultValidation = "The resulting value was expected to be zero, but {0} was found.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}
