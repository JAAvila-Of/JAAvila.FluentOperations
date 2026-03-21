namespace JAAvila.FluentOperations.Common;

/// <summary>
/// Represents a set of predefined constant keys that serve as identifiers for template components.
/// These keys are used to define and organize parts of a template, such as subject, value, expected outcome, reason, and rule.
/// The full template will be exposed if the operation fails and an exception is generated.
/// </summary>
internal struct TemplatePart
{
    public const string Subject = "Subject";
    public const string Value = "Value";
    public const string Expected = "Expected";
    public const string Result = "Result";
    public const string Reason = "Reason";
    public const string Rule = "Rule";
    public const string Expression = "Expression";
    public const string Fail = "Fail";
    public const string Transaction = "Transaction";
    public const string ErrorCode = "ErrorCode";
    public const string SeverityLevel = "Severity";
    public const string Diff = "Diff";
}
