namespace JAAvila.FluentOperations;

/// <summary>
/// Provides a scope for batching multiple inline assertion failures.
/// All assertions inside the scope are collected; on Dispose, a combined
/// exception is thrown if any failures were accumulated.
/// </summary>
public sealed class AssertionScope : IDisposable
{
    private readonly TransactionalOperations _transactional;

    /// <summary>
    /// Initializes a new <see cref="AssertionScope"/> with no context name.
    /// </summary>
    public AssertionScope()
    {
        _transactional = new TransactionalOperations(Common.TransactionalMode.AccumulateFailsAndDisposeThis);
    }

    /// <summary>
    /// Initializes a new <see cref="AssertionScope"/> with the given context name.
    /// </summary>
    /// <param name="name">A descriptive name for this scope, included in failure reports.</param>
    public AssertionScope(string name)
    {
        _transactional = new TransactionalOperations(name, Common.TransactionalMode.AccumulateFailsAndDisposeThis);
    }

    /// <summary>
    /// Returns true if any assertion failures have been accumulated in this scope.
    /// </summary>
    public bool HasFailures => _transactional.GetTemplates().Count > 0;

    /// <summary>
    /// Returns the list of accumulated failure messages in this scope.
    /// </summary>
    public IReadOnlyList<string> FailureMessages => _transactional.GetTemplates();

    /// <summary>
    /// Adds a custom failure message to this scope without involving a manager.
    /// </summary>
    /// <param name="message">The failure message to record.</param>
    public void FailWith(string message)
    {
        _transactional.HandleAddTemplate(message);
    }

    /// <summary>
    /// Clears all accumulated failures, preventing an exception from being thrown on Dispose.
    /// </summary>
    public void Discard()
    {
        _transactional.ClearTemplates();
    }

    /// <summary>
    /// Disposes the scope. If any failures were accumulated and not discarded, throws
    /// a combined exception listing all failures.
    /// </summary>
    public void Dispose()
    {
        _transactional.Dispose();
    }
}
