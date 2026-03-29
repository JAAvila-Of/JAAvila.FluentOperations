using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace JAAvila.FluentOperations.Analyzers;

/// <summary>
/// FO003 — Reports a warning when <c>Define()</c> is called inside a QualityBlueprint
/// without being wrapped in a <c>using</c> statement.
/// </summary>
[DiagnosticAnalyzer(LanguageNames.CSharp)]
public sealed class DefineWithoutUsingAnalyzer : DiagnosticAnalyzer
{
    private const string QualityBlueprintTypeName = "QualityBlueprint";
    private const string QualityBlueprintFullName = "JAAvila.FluentOperations.QualityBlueprint";

    /// <summary>
    /// Gets an immutable array of <see cref="DiagnosticDescriptor"/> objects that the analyzer is capable of producing.
    /// </summary>
    /// <remarks>
    /// This property defines the diagnostics that the <see cref="DefineWithoutUsingAnalyzer"/> can report.
    /// Each diagnostic descriptor provides details such as the unique identifier, title, message format,
    /// category, default severity, and whether the diagnostic is enabled by default.
    /// </remarks>
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics =>
        ImmutableArray.Create(DiagnosticIds.DefineWithoutUsing);

    /// <summary>
    /// Registers actions to analyze a C# syntax tree, enabling detection and reporting
    /// of specific diagnostic issues within the analyzed code.
    /// </summary>
    /// <param name="context">
    /// The context in which analysis actions are registered. This provides mechanisms
    /// to configure analysis behavior and specify the actions to be executed.
    /// </param>
    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();
        context.RegisterSyntaxNodeAction(AnalyzeInvocation, SyntaxKind.InvocationExpression);
    }

    private static void AnalyzeInvocation(SyntaxNodeAnalysisContext context)
    {
        var invocation = (InvocationExpressionSyntax)context.Node;

        // Must be a simple name invocation "Define()" or member access "this.Define()"
        var methodName = invocation.Expression switch
        {
            SimpleNameSyntax simple => simple.Identifier.Text,
            MemberAccessExpressionSyntax ma => ma.Name.Identifier.Text,
            _ => null
        };

        if (methodName != "Define")
        {
            return;
        }

        // Must have 0 arguments
        if (invocation.ArgumentList.Arguments.Count != 0)
        {
            return;
        }

        // Resolve the method symbol
        var symbolInfo = context.SemanticModel.GetSymbolInfo(invocation, context.CancellationToken);

        if (symbolInfo.Symbol is not IMethodSymbol methodSymbol)
        {
            return;
        }

        // The method must be defined in a type that inherits from QualityBlueprint<T>
        if (!IsDefinedInQualityBlueprint(methodSymbol.ContainingType))
        {
            return;
        }

        // Check if this invocation is already wrapped in a using statement
        if (IsWrappedInUsing(invocation))
        {
            return;
        }

        // Define() is not inside a using — report FO003
        context.ReportDiagnostic(
            Diagnostic.Create(DiagnosticIds.DefineWithoutUsing, invocation.GetLocation())
        );
    }

    /// <summary>
    /// Walks the inheritance chain of <paramref name="type"/> to determine if it
    /// inherits from <c>QualityBlueprint&lt;T&gt;</c>.
    /// </summary>
    private static bool IsDefinedInQualityBlueprint(INamedTypeSymbol? type)
    {
        var current = type;

        while (current != null)
        {
            var fullName = current.ConstructedFrom?.ToDisplayString() ?? current.ToDisplayString();

            // Match "JAAvila.FluentOperations.QualityBlueprint<T>" or unbound "QualityBlueprint"
            if (fullName.StartsWith(QualityBlueprintFullName, System.StringComparison.Ordinal))
            {
                return true;
            }

            // Fallback: match by simple name in case the namespace is aliased
            if (current.Name == QualityBlueprintTypeName)
            {
                return true;
            }

            current = current.BaseType;
        }

        return false;
    }

    /// <summary>
    /// Returns <c>true</c> if the invocation node is wrapped by any of the three
    /// supported using forms:
    /// <list type="bullet">
    ///   <item><c>using (Define()) { }</c></item>
    ///   <item><c>using (var x = Define()) { }</c></item>
    ///   <item><c>using var x = Define();</c></item>
    /// </list>
    /// </summary>
    private static bool IsWrappedInUsing(InvocationExpressionSyntax invocation)
    {
        var parent = invocation.Parent;

        // Form 1: using (Define()) { }
        // The invocation is the resource expression of a UsingStatementSyntax
        if (parent is UsingStatementSyntax)
        {
            return true;
        }

        // Form 2: using (var x = Define()) { }
        // invocation → EqualsValueClauseSyntax → VariableDeclaratorSyntax
        //            → VariableDeclarationSyntax → UsingStatementSyntax
        if (parent is EqualsValueClauseSyntax equalsClause)
        {
            if (
                equalsClause.Parent is VariableDeclaratorSyntax declarator
                && declarator.Parent is VariableDeclarationSyntax declaration
                && declaration.Parent is UsingStatementSyntax
            )
            {
                return true;
            }

            // Form 3: using var x = Define();
            // invocation → EqualsValueClauseSyntax → VariableDeclaratorSyntax
            //            → VariableDeclarationSyntax → LocalDeclarationStatementSyntax (with UsingKeyword)
            if (
                equalsClause.Parent is VariableDeclaratorSyntax declarator2
                && declarator2.Parent is VariableDeclarationSyntax declaration2
                && declaration2.Parent is LocalDeclarationStatementSyntax localDecl
                && localDecl.UsingKeyword.IsKind(SyntaxKind.UsingKeyword)
            )
            {
                return true;
            }
        }

        return false;
    }
}
