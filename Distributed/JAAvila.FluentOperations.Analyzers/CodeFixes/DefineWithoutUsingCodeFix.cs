using System.Collections.Generic;
using System.Collections.Immutable;
using System.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Formatting;

namespace JAAvila.FluentOperations.Analyzers;

/// <summary>
/// Code fix for FO003: wraps a bare <c>Define()</c> call in <c>using (Define()) { ... }</c>,
/// moving all later sibling statements inside the new using block.
/// </summary>
[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(DefineWithoutUsingCodeFix))]
[Shared]
public sealed class DefineWithoutUsingCodeFix : CodeFixProvider
{
    /// <summary>
    /// Gets an immutable array of diagnostic IDs that the code fix provider can address.
    /// These diagnostic IDs represent specific issues for which the provider offers code fixes.
    /// In this case, it includes the diagnostic ID associated with the "Define() called without using" rule,
    /// which ensures that calls to <c>Define()</c> are properly wrapped in a <c>using</c> statement
    /// to manage the capture scope lifecycle correctly.
    /// </summary>
    public override ImmutableArray<string> FixableDiagnosticIds =>
        ImmutableArray.Create(DiagnosticIds.DefineWithoutUsing.Id);

    /// <summary>
    /// Retrieves the <see cref="FixAllProvider"/> that provides the ability to fix all occurrences
    /// of the diagnostic(s) handled by this code fix provider in all documents or projects.
    /// </summary>
    /// <returns>
    /// A <see cref="FixAllProvider"/> instance that performs batch fixes for all diagnostics
    /// of the types handled by this provider, or <c>null</c> if batch fixing is not supported.
    /// </returns>
    public override FixAllProvider GetFixAllProvider() => WellKnownFixAllProviders.BatchFixer;

    /// <summary>
    /// Registers code fixes that address diagnostics in the document by wrapping a
    /// bare <c>Define()</c> call in a <c>using</c> statement and moving later
    /// statements inside the newly created block.
    /// </summary>
    /// <param name="context">
    /// A <see cref="CodeFixContext"/> object containing the diagnostic information
    /// and the context in which the code fix is registered.
    /// </param>
    /// <returns>
    /// A <see cref="Task"/> that represents the asynchronous operation of registering
    /// code fixes.
    /// </returns>
    public override async Task RegisterCodeFixesAsync(CodeFixContext context)
    {
        var root = await context
            .Document.GetSyntaxRootAsync(context.CancellationToken)
            .ConfigureAwait(false);

        if (root is null)
        {
            return;
        }

        var diagnostic = context.Diagnostics.First();
        var diagnosticSpan = diagnostic.Location.SourceSpan;

        // Find the InvocationExpressionSyntax for Define()
        var invocation = root.FindNode(diagnosticSpan)
            .FirstAncestorOrSelf<InvocationExpressionSyntax>();

        if (invocation is null)
        {
            return;
        }

        context.RegisterCodeFix(
            CodeAction.Create(
                title: "Wrap Define() in using statement",
                createChangedDocument: ct => WrapInUsingAsync(context.Document, invocation, ct),
                equivalenceKey: "FO003_WrapInUsing"
            ),
            diagnostic
        );
    }

    private static async Task<Document> WrapInUsingAsync(
        Document document,
        InvocationExpressionSyntax defineInvocation,
        CancellationToken cancellationToken
    )
    {
        var root = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);

        if (root is null)
        {
            return document;
        }

        // The bare Define() call sits inside an ExpressionStatementSyntax
        var exprStatement = defineInvocation.FirstAncestorOrSelf<ExpressionStatementSyntax>();

        if (exprStatement is null)
        {
            return document;
        }

        // The statement must live directly inside a block (e.g., constructor body)
        if (exprStatement.Parent is not BlockSyntax block)
        {
            return document;
        }

        var statements = block.Statements;
        var defineIndex = statements.IndexOf(exprStatement);

        if (defineIndex < 0)
        {
            return document;
        }

        // All statements that come AFTER Define() in the same block move inside the using body
        var innerStatements = new List<StatementSyntax>();

        for (var i = defineIndex + 1; i < statements.Count; i++)
        {
            innerStatements.Add(statements[i].WithoutLeadingTrivia());
        }

        // Build: using (Define()) { <innerStatements> }
        var usingBody = SyntaxFactory.Block(SyntaxFactory.List(innerStatements));

        var usingStatement = SyntaxFactory
            .UsingStatement(
                declaration: null,
                expression: defineInvocation.WithoutTrivia(),
                statement: usingBody
            )
            .WithAdditionalAnnotations(Formatter.Annotation)
            .WithLeadingTrivia(exprStatement.GetLeadingTrivia());

        // Remove the original Define() statement and all statements that moved inside
        var statementsToRemove = new HashSet<StatementSyntax> { exprStatement };

        for (var i = defineIndex + 1; i < statements.Count; i++)
        {
            statementsToRemove.Add(statements[i]);
        }

        var newStatements = statements.Where(s => !statementsToRemove.Contains(s)).ToList();

        newStatements.Add(usingStatement);

        var newBlock = block.WithStatements(SyntaxFactory.List(newStatements));
        var newRoot = root.ReplaceNode(block, newBlock);

        return document.WithSyntaxRoot(newRoot);
    }
}
