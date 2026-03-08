using JAAvila.FluentOperations.Contract;
using JAAvila.FluentOperations.Model;

namespace JAAvila.FluentOperations;

public abstract partial class QualityBlueprint<T>
    where T : notnull
{
    /// <summary>
    /// Registers conditional validation rules.
    /// Rules defined within the 'then' action will only be evaluated
    /// when the condition returns true for the model instance during Check().
    /// </summary>
    protected void When(Func<T, bool> condition, Action then, Action? otherwise = null)
    {
        // Create a shared ConditionGroup for all rules in this When() group
        var group = new ConditionGroup<T>(condition);
        _conditionGroups.Add(group);

        // Save previously captured rules
        var previousCollected = _capturedDuringDefinition.ToList();
        _capturedDuringDefinition.Clear();

        // Capture rules for 'then' branch
        var thenRules = new List<IQualityRule>();

        RuleCaptureContext.BeginPropertyCapture(
            _capturedDuringDefinition,
            RuleCaptureContext.CurrentPropertyName ?? string.Empty,
            _currentScenario
        );

        then();

        thenRules.AddRange(_capturedDuringDefinition);
        _capturedDuringDefinition.Clear();

        // Capture rules for 'otherwise' branch (if provided)
        var otherwiseRules = new List<IQualityRule>();
        if (otherwise is not null)
        {
            RuleCaptureContext.BeginPropertyCapture(
                _capturedDuringDefinition,
                RuleCaptureContext.CurrentPropertyName ?? string.Empty,
                _currentScenario
            );

            otherwise();

            otherwiseRules.AddRange(_capturedDuringDefinition);
            _capturedDuringDefinition.Clear();
        }

        // Restore previously captured rules
        _capturedDuringDefinition.AddRange(previousCollected);

        // Wrap each captured rule with the shared ConditionGroup
        foreach (var rule in thenRules)
        {
            if (rule is not CapturedRule captured)
            {
                continue;
            }

            var wrapped = new ConditionalRuleWrapper<T>(
                captured,
                condition,
                isOtherwise: false,
                conditionGroup: group
            );
            _capturedDuringDefinition.Add(
                new CapturedRule(captured.PropertyName, wrapped, captured.Scenario, captured.Config)
            );

            // Ensure the extractor is registered
            if (!_extractors.ContainsKey(captured.PropertyName))
            {
                _extractors[captured.PropertyName] = _ => null;
            }
        }

        foreach (var rule in otherwiseRules)
        {
            if (rule is not CapturedRule captured)
            {
                continue;
            }

            var wrapped = new ConditionalRuleWrapper<T>(
                captured,
                condition,
                isOtherwise: true,
                conditionGroup: group
            );
            _capturedDuringDefinition.Add(
                new CapturedRule(captured.PropertyName, wrapped, captured.Scenario, captured.Config)
            );

            if (!_extractors.ContainsKey(captured.PropertyName))
            {
                _extractors[captured.PropertyName] = _ => null;
            }
        }
    }
}
