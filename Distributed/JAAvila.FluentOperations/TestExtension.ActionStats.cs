using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using JAAvila.FluentOperations.Manager;
using JAAvila.FluentOperations.Model;

namespace JAAvila.FluentOperations;

public static partial class TestExtension
{
    /// <summary>
    /// Initializes a new instance of <see cref="ActionStatsOperationsManager"/>,
    /// allowing fluent assertions on captured execution statistics.
    /// </summary>
    /// <param name="stats">The captured execution statistics to assert on.</param>
    /// <param name="callerName">The caller expression name, captured automatically.</param>
    /// <returns>An instance of <see cref="ActionStatsOperationsManager"/> for fluent assertion chaining.</returns>
    [Pure]
    public static ActionStatsOperationsManager Test(
        this ActionStats? stats,
        [CallerArgumentExpression("stats")] string callerName = ""
    )
    {
        return new ActionStatsOperationsManager(stats, callerName);
    }
}
