using Microsoft.CodeAnalysis;

namespace JAAvila.FluentOperations.Analyzers
{
    internal static class DiagnosticIds
    {
        internal static readonly DiagnosticDescriptor DanglingTestChain = new DiagnosticDescriptor(
            id: "FO001",
            title: "Dangling .Test() chain",
            messageFormat: "'.Test()' is called but no assertion operation is chained. The result is discarded and no validation occurs.",
            category: "Usage",
            defaultSeverity: DiagnosticSeverity.Warning,
            isEnabledByDefault: true,
            description: "Calling '.Test()' without chaining an assertion operation (e.g., '.NotBeNull()', '.BePositive()') discards the result and performs no validation.");

        internal static readonly DiagnosticDescriptor DefineWithoutUsing = new DiagnosticDescriptor(
            id: "FO003",
            title: "Define() called without using",
            messageFormat: "'Define()' must be wrapped in a 'using' statement to ensure the rule-capture scope is disposed correctly.",
            category: "Usage",
            defaultSeverity: DiagnosticSeverity.Warning,
            isEnabledByDefault: true,
            description: "Calling 'Define()' in a QualityBlueprint constructor without wrapping it in a 'using' statement leaves the capture context open, which may cause rules to be captured into the wrong blueprint.");
    }
}
