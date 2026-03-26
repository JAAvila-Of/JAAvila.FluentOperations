using JAAvila.FluentOperations.Contract;
using JAAvila.FluentOperations.Model;

namespace JAAvila.FluentOperations;

public abstract partial class QualityBlueprint<T>
{
    /// <summary>
    /// Returns a flat list of <see cref="BlueprintRuleInfo"/> describing every rule registered
    /// in this blueprint, including rules inside <c>ForEach</c> definitions.
    /// </summary>
    /// <remarks>
    /// Rules inside <c>ForNested</c> definitions are not enumerated because they delegate to
    /// a child blueprint with its own <see cref="GetRuleDescriptors"/> call.
    /// </remarks>
    /// <returns>
    /// A read-only list of <see cref="BlueprintRuleInfo"/> ordered by registration sequence.
    /// </returns>
    public IReadOnlyList<BlueprintRuleInfo> GetRuleDescriptors()
    {
        var result = new List<BlueprintRuleInfo>();

        // Normal property definitions
        foreach (var def in _ruleDefinitions)
        {
            foreach (var rule in def.Rules)
            {
                var info = ExtractRuleInfo(
                    rule,
                    def.PropertyName,
                    def.Scenario,
                    def.RuleSet
                );
                if (info is not null)
                {
                    result.Add(info);
                }
            }
        }

        // ForEach definitions — property name is suffixed with [] to signal collection context
        foreach (var def in _forEachDefinitions)
        {
            var collectionPropertyName = $"{def.PropertyName}[]";
            foreach (var rule in def.Rules)
            {
                var info = ExtractRuleInfo(
                    rule,
                    collectionPropertyName,
                    def.Scenario,
                    def.RuleSet
                );
                if (info is not null)
                {
                    result.Add(info);
                }
            }
        }

        return result.AsReadOnly();
    }

    /// <summary>
    /// Extracts a <see cref="BlueprintRuleInfo"/> from a captured rule.
    /// Unwraps <see cref="CapturedRule"/> decorators and queries <see cref="IRuleDescriptor"/>
    /// when available, falling back to <see cref="IValidator.MessageKey"/> otherwise.
    /// </summary>
    private static BlueprintRuleInfo? ExtractRuleInfo(
        IQualityRule rule,
        string propertyName,
        Type? scenario,
        string? ruleSet = null)
    {
        // Unwrap CapturedRule decorator to access the inner engine and its config
        var capturedRule = rule as CapturedRule;
        var innerRule = capturedRule?.Inner ?? rule;
        var config = capturedRule?.Config;

        // Determine severity and error code — prefer CapturedRule.Config values
        var severity = config?.Severity ?? Severity.Error;
        var errorCode = config?.ErrorCode;

        // Try to get structured descriptor from the inner rule (ExecutionEngine implements IRuleDescriptor)
        if (innerRule is IRuleDescriptor descriptor)
        {
            return new BlueprintRuleInfo
            {
                PropertyName = propertyName,
                OperationName = descriptor.OperationName,
                PropertyType = descriptor.SubjectType,
                Parameters = descriptor.Parameters,
                Severity = severity,
                ErrorCode = errorCode,
                Scenario = scenario,
                RuleSetName = ruleSet,
            };
        }

        // Fallback for any IQualityRule that does not implement IRuleDescriptor
        // (CustomValidatorRule, AsyncPredicateRule, etc.)
        return new BlueprintRuleInfo
        {
            PropertyName = propertyName,
            OperationName = innerRule.GetType().Name,
            PropertyType = typeof(object),
            Severity = severity,
            ErrorCode = errorCode,
            Scenario = scenario,
            RuleSetName = ruleSet,
        };
    }
}
