namespace JAAvila.FluentOperations.Common;

/// <summary>
/// Controls how the execution engine handles assertion failures and when it disposes its transactional context.
/// </summary>
public enum TransactionalMode
{
    /// <summary>
    /// Stops execution on the first failure and disposes the current transactional context.
    /// </summary>
    FirstFailAndDisposeThis,

    /// <summary>
    /// Stops execution on the first failure and disposes all transactional contexts in the current scope.
    /// </summary>
    FirstFailAndDisposeAll,

    /// <summary>
    /// Accumulates all failures before disposing the current transactional context.
    /// </summary>
    AccumulateFailsAndDisposeThis,

    /// <summary>
    /// Accumulates all failures before disposing all transactional contexts in the current scope.
    /// </summary>
    AccumulateFailsAndDisposeAll
}
