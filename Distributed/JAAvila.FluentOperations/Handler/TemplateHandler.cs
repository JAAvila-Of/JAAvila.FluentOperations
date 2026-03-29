using System.Diagnostics.CodeAnalysis;
using System.Text;
using JAAvila.FluentOperations.Common;
using JAAvila.FluentOperations.Config;
using JAAvila.FluentOperations.Formatters;
using JAAvila.FluentOperations.Model;

// ReSharper disable once RedundantUsingDirective

namespace JAAvila.FluentOperations.Handler;

/// <summary>
/// Builds structured failure message strings by accumulating labeled template parts
/// (subject, expected value, actual result, failure reason, rule name, severity, error code, etc.)
/// and concatenating them into a single formatted string via the <see cref="Result"/> property.
/// When a <see cref="TransactionalOperations"/> scope is active, the transaction name is
/// automatically prepended to every generated message.
/// </summary>
internal class TemplateHandler
{
    //private const string Result = "";
    private readonly List<KeyValuePair<string, Template>> _templates = [];
    //private readonly string _default = "the operation has failed";

    /// <summary>
    /// Appends a reason part to the message. No-ops when <paramref name="reason"/> is <c>null</c>.
    /// </summary>
    /// <param name="reason">A composite-format string explaining why the assertion was triggered.</param>
    /// <param name="arguments">Optional format arguments for <paramref name="reason"/>.</param>
    /// <returns>The current handler instance for fluent chaining.</returns>
    public TemplateHandler WithReason(
        [StringSyntax("CompositeFormat")] string? reason,
        params object[] arguments
    )
    {
        if (reason is not null)
        {
            _templates.Add(
                new KeyValuePair<string, Template>(
                    TemplatePart.Reason,
                    Template.New(reason, arguments)
                )
            );
        }

        return this;
    }

    /// <summary>
    /// Appends the subject (property name or caller expression) to the message.
    /// </summary>
    /// <param name="subject">A composite-format string identifying the value under test.</param>
    /// <param name="arguments">Optional format arguments for <paramref name="subject"/>.</param>
    /// <returns>The current handler instance for fluent chaining.</returns>
    public TemplateHandler WithSubject(
        [StringSyntax("CompositeFormat")] string subject,
        params object[] arguments
    )
    {
        _templates.Add(
            new KeyValuePair<string, Template>(
                TemplatePart.Subject,
                Template.New(subject, arguments)
            )
        );
        return this;
    }

    public TemplateHandler WithValue(
        [StringSyntax("CompositeFormat")] string value,
        params object[] arguments
    )
    {
        _templates.Add(
            new KeyValuePair<string, Template>(TemplatePart.Value, Template.New(value, arguments))
        );
        return this;
    }

    /// <summary>
    /// Appends a precondition-failure description to the message (used by <c>FailIf</c> conditions).
    /// </summary>
    /// <param name="value">A composite-format string describing the precondition that fired.</param>
    /// <param name="arguments">Optional format arguments for <paramref name="value"/>.</param>
    /// <returns>The current handler instance for fluent chaining.</returns>
    public TemplateHandler WithFail(
        [StringSyntax("CompositeFormat")] string value,
        params object[] arguments
    )
    {
        _templates.Add(
            new KeyValuePair<string, Template>(TemplatePart.Fail, Template.New(value, arguments))
        );
        return this;
    }

    public TemplateHandler WithExpected(
        [StringSyntax("CompositeFormat")] string expected,
        params object[] arguments
    )
    {
        _templates.Add(
            new KeyValuePair<string, Template>(
                TemplatePart.Expected,
                Template.New(expected, arguments)
            )
        );
        return this;
    }

    public TemplateHandler WithResult(
        [StringSyntax("CompositeFormat")] string? expected,
        params object[] arguments
    )
    {
        if (expected is not null)
        {
            _templates.Add(
                new KeyValuePair<string, Template>(
                    TemplatePart.Result,
                    Template.New(expected, arguments)
                )
            );
        }

        return this;
    }

    /// <summary>
    /// Appends the result/failure description to the message, preferring a localized message when available.
    /// Falls back to <paramref name="resultValidation"/> when no provider is configured or the key is not found.
    /// </summary>
    /// <param name="messageKey">The stable key used to look up the localized message (e.g., "String.BeEmail").</param>
    /// <param name="resultValidation">The default (English) composite-format failure message.</param>
    /// <param name="arguments">Optional format arguments applied to whichever message string is used.</param>
    /// <returns>The current handler instance for fluent chaining.</returns>
    public TemplateHandler WithResult(
        string messageKey,
        [StringSyntax("CompositeFormat")] string? resultValidation,
        params object[] arguments
    )
    {
        var localizationConfig = GlobalConfig.GetLocalizationConfig();
        var localizedMessage = localizationConfig?.ResolveMessage(messageKey);
        var finalMessage = localizedMessage ?? resultValidation;

        if (finalMessage is not null)
        {
            _templates.Add(
                new KeyValuePair<string, Template>(
                    TemplatePart.Result,
                    Template.New(finalMessage, arguments)
                )
            );
        }

        return this;
    }

    /// <summary>
    /// Appends the rule/operation name to the message (e.g., the enum string value of the operation).
    /// </summary>
    /// <param name="rule">A composite-format string identifying the rule that failed.</param>
    /// <param name="arguments">Optional format arguments for <paramref name="rule"/>.</param>
    /// <returns>The current handler instance for fluent chaining.</returns>
    public TemplateHandler WithRule(
        [StringSyntax("CompositeFormat")] string rule,
        params object[] arguments
    )
    {
        _templates.Add(
            new KeyValuePair<string, Template>(TemplatePart.Rule, Template.New(rule, arguments))
        );
        return this;
    }

    public TemplateHandler WithExpression(
        [StringSyntax("CompositeFormat")] string expression,
        params object[] arguments
    )
    {
        _templates.Add(
            new KeyValuePair<string, Template>(
                TemplatePart.Expression,
                Template.New(expression, arguments)
            )
        );
        return this;
    }

    public TemplateHandler WithErrorCode(string? errorCode)
    {
        if (errorCode is not null)
        {
            _templates.Add(
                new KeyValuePair<string, Template>(TemplatePart.ErrorCode, Template.New(errorCode))
            );
        }
        
        return this;
    }

    public TemplateHandler WithSeverity(Severity severity)
    {
        if (severity != Severity.Error) // Only add if non-default
        {
            _templates.Add(
                new KeyValuePair<string, Template>(
                    TemplatePart.SeverityLevel,
                    Template.New(severity.ToString())
                )
            );
        }
        
        return this;
    }

    public TemplateHandler WithStringDiff(string? expected, string? actual)
    {
        var config = GlobalConfig.GetStringConfig();
        
        if (!config.EnableStringDiff)
        {
            return this;
        }

        var diff = StringDiffFormatter.FormatDiff(expected, actual, config.StringDiffContextChars, config.StringDiffMaxLength);
        
        if (diff is not null)
        {
            _templates.Add(new KeyValuePair<string, Template>(TemplatePart.Diff, Template.New(diff)));
        }
        
        return this;
    }

    public TemplateHandler WithTransaction(TransactionalOperations transactional)
    {
        _templates.Insert(
            0,
            new KeyValuePair<string, Template>(
                TemplatePart.Transaction,
                Template.New(transactional.Name)
            )
        );
        return this;
    }

    /// <summary>
    /// Gets the fully assembled failure message string by concatenating all accumulated template parts.
    /// If a <see cref="TransactionalOperations"/> scope is active, its name is prepended automatically.
    /// Returns an empty string when no parts have been added.
    /// </summary>
    public string Result
    {
        get
        {
            if (_templates.Count == 0)
            {
                return string.Empty;
            }

            var transaction = TransactionalOperations.Current;

            if (transaction is not null)
            {
                WithTransaction(transaction);
            }

            var sb = new StringBuilder();

            foreach (var template in _templates)
            {
                sb.Append(template.Key);
                sb.Append(": ");
                sb.Append(template.Value);
                sb.AppendLine();
            }

            return sb.ToString().TrimEnd();
        }
    }
}
