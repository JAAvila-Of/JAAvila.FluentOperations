using JAAvila.FluentOperations.Contract;
using JAAvila.SafeTypes.Extension;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the nullable double value equals itself when rounded to the specified decimal places.
/// </summary>
internal class NullableDoubleBeRoundedToValidator(PrincipalChain<double?> chain, int decimals)
    : IValidator,
        IRuleDescriptor
{
    public static NullableDoubleBeRoundedToValidator New(
        PrincipalChain<double?> chain,
        int decimals
    ) => new(chain, decimals);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "NullableDouble.BeRoundedTo";
    string IRuleDescriptor.OperationName => "BeRoundedTo";
    Type IRuleDescriptor.SubjectType => typeof(double?);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object> { ["value"] = decimals };

    public bool Validate()
    {
        var value = chain.GetValue().SafeNull();

        if (Math.Abs(Math.Round(value, decimals) - value) < 1e-10)
        {
            return true;
        }

        ResultValidation =
            "The resulting value was expected to be rounded to {0} decimal places, but it was not.";
        return false;
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}
