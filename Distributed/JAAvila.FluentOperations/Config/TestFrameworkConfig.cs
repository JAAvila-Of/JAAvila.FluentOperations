using JAAvila.FluentOperations.Common;

namespace JAAvila.FluentOperations.Config;

/// <summary>
/// Configuration for the test framework used to throw assertion exceptions.
/// </summary>
public class TestFrameworkConfig
{
    /// <summary>
    /// The test framework to use for assertion exceptions.
    /// Default: <see cref="TestFramework.Auto"/> (auto-detect from loaded assemblies).
    /// </summary>
    public TestFramework Framework { get; init; } = TestFramework.Auto;
}
