namespace JAAvila.FluentOperations.Architecture;

/// <summary>
/// Static provider that manages the active <see cref="IDependencyScanner"/> instance.
/// By default, uses the reflection-based <see cref="ReflectionDependencyScanner"/>.
/// External packages (e.g., JAAvila.FluentOperations.Architecture) can register
/// an enhanced scanner via <see cref="SetScanner"/>.
/// </summary>
public static class DependencyScannerProvider
{
    private static IDependencyScanner _scanner = ReflectionDependencyScanner.Instance;

    /// <summary>
    /// Returns the currently active dependency scanner.
    /// </summary>
    public static IDependencyScanner Current => _scanner;

    /// <summary>
    /// Replaces the active dependency scanner. Used by extension packages
    /// to provide enhanced scanning capabilities (e.g., IL-level analysis).
    /// </summary>
    /// <param name="scanner">The scanner to use. Must not be null.</param>
    public static void SetScanner(IDependencyScanner scanner)
    {
        ArgumentNullException.ThrowIfNull(scanner);
        _scanner = scanner;
    }

    /// <summary>
    /// Resets the scanner to the default reflection-based implementation.
    /// Intended for test teardown.
    /// </summary>
    public static void Reset()
    {
        _scanner = ReflectionDependencyScanner.Instance;
    }
}
