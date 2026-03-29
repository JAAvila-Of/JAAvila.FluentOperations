using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace JAAvila.FluentOperations.Analyzers;

/// <summary>
/// FO001 — Reports a warning when <c>.Test()</c> is called as a standalone expression statement
/// without chaining an assertion operation. The manager result is discarded and no validation runs.
/// </summary>
[DiagnosticAnalyzer(LanguageNames.CSharp)]
public sealed class DanglingTestChainAnalyzer : DiagnosticAnalyzer
{
    /// <summary>
    /// Gets the collection of <see cref="DiagnosticDescriptor"/> instances supported by the analyzer.
    /// </summary>
    /// <remarks>
    /// The property is overridden to provide the specific diagnostics that this analyzer reports.
    /// For the <see cref="DanglingTestChainAnalyzer"/>, it includes the diagnostic descriptor
    /// for reporting a warning about standalone usage of the <c>.Test()</c> method without
    /// later assertion chaining.
    /// </remarks>
    /// <returns>
    /// An <see cref="ImmutableArray{T}"/> of <see cref="DiagnosticDescriptor"/> instances.
    /// This analyzer supports diagnostics such as "FO001 - Dangling .Test() chain."
    /// </returns>
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics =>
        ImmutableArray.Create(DiagnosticIds.DanglingTestChain);

    /// <summary>
    /// Initializes the <see cref="DanglingTestChainAnalyzer"/> by configuring its analysis settings and registering actions
    /// to analyze specific syntax nodes within the source code.
    /// </summary>
    /// <param name="context">
    /// An <see cref="AnalysisContext"/> object that provides methods for initializing and configuring analysis behavior.
    /// This includes registering actions to be executed during various stages of the analysis.
    /// </param>
    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();
        context.RegisterSyntaxNodeAction(
            AnalyzeExpressionStatement,
            SyntaxKind.ExpressionStatement
        );
    }

    private static void AnalyzeExpressionStatement(SyntaxNodeAnalysisContext context)
    {
        var expressionStatement = (ExpressionStatementSyntax)context.Node;

        // The full statement must be an invocation expression (not a member-access chain)
        if (expressionStatement.Expression is not InvocationExpressionSyntax invocation)
        {
            return;
        }

        // The invocation must be a member access: <expr>.Test()
        if (invocation.Expression is not MemberAccessExpressionSyntax memberAccess)
        {
            return;
        }

        // The method name must be "Test"
        if (memberAccess.Name.Identifier.Text != "Test")
        {
            return;
        }

        // Resolve the method symbol to confirm it returns a FluentOperations manager
        var symbolInfo = context.SemanticModel.GetSymbolInfo(invocation, context.CancellationToken);

        if (symbolInfo.Symbol is not IMethodSymbol methodSymbol)
        {
            return;
        }

        // Check the return type is in the JAAvila.FluentOperations.Manager namespace
        // and its name ends with "OperationsManager"
        var returnType = methodSymbol.ReturnType;

        if (returnType is null)
        {
            return;
        }

        var namespaceName = returnType.ContainingNamespace?.ToDisplayString() ?? string.Empty;
        var typeName = returnType.Name;

        if (
            !namespaceName.StartsWith(
                "JAAvila.FluentOperations.Manager",
                System.StringComparison.Ordinal
            )
        )
        {
            return;
        }

        if (!typeName.EndsWith("OperationsManager", System.StringComparison.Ordinal))
        {
            return;
        }

        // The .Test() result is being discarded — report FO001
        context.ReportDiagnostic(
            Diagnostic.Create(DiagnosticIds.DanglingTestChain, memberAccess.Name.GetLocation())
        );
    }
}
