namespace JAAvila.FluentOperations.Common;

/// <summary>
/// Represents the outcome of a transactional assertion execution.
/// </summary>
public enum TransactionalStatus
{
    /// <summary>
    /// All assertions passed without failures.
    /// </summary>
    Success,

    /// <summary>
    /// Execution was stopped early because a failure was encountered and the mode was set to fail-first.
    /// </summary>
    Interrupted,

    /// <summary>
    /// One or more assertions failed.
    /// </summary>
    Fail
}
