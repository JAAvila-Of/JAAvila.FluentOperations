namespace JAAvila.FluentOperations.Architecture;

/// <summary>
/// Static configuration for architecture-level testing features.
/// Call <see cref="UseCecilDependencyScanning"/> to activate IL-level dependency
/// detection for <c>HaveDependencyOn</c>, <c>NotHaveDependencyOn</c>, and
/// <c>OnlyHaveDependenciesOn</c> operations.
/// </summary>
public static class ArchitectureScannerConfig
{
    /// <summary>
    /// Activates Mono.Cecil-based IL scanning for dependency detection.
    /// When active, <c>HaveDependencyOn</c> and related operations detect
    /// dependencies in method bodies (local variables, <c>new</c> calls,
    /// <c>typeof()</c>, casts, method calls, field access, catch blocks).
    /// </summary>
    /// <remarks>
    /// Call this once in test setup (e.g., <c>[OneTimeSetUp]</c> or
    /// <c>[AssemblyInitialize]</c>). Call <see cref="Reset"/> in teardown.
    /// </remarks>
    /// <returns>The <see cref="CecilDependencyScanner"/> instance for advanced usage.</returns>
    public static CecilDependencyScanner UseCecilDependencyScanning()
    {
        var scanner = new CecilDependencyScanner();
        DependencyScannerProvider.SetScanner(scanner);
        return scanner;
    }

    /// <summary>
    /// Resets dependency scanning to the default reflection-based implementation
    /// and disposes the Cecil scanner (releasing cached assemblies).
    /// </summary>
    public static void Reset()
    {
        var current = DependencyScannerProvider.Current;
        DependencyScannerProvider.Reset();

        if (current is IDisposable disposable)
        {
            disposable.Dispose();
        }
    }
}
