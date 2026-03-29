using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the float value equals itself when rounded to the specified decimal places.
/// </summary>
internal class FloatBeRoundedToValidator(PrincipalChain<float> chain, int decimals)
    : IValidator,
        IRuleDescriptor
{
    public static FloatBeRoundedToValidator New(PrincipalChain<float> chain, int decimals) =>
        new(chain, decimals);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "Float.BeRoundedTo";
    string IRuleDescriptor.OperationName => "BeRoundedTo";
    Type IRuleDescriptor.SubjectType => typeof(float);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object> { ["value"] = decimals };

    public bool Validate()
    {
        var value = chain.GetValue();

        if (Math.Abs((float)Math.Round(value, decimals) - value) < 1e-6f)
        {
            return true;
        }

        ResultValidation =
            "The resulting value was expected to be rounded to {0} decimal places, but {1} was found.";
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}
