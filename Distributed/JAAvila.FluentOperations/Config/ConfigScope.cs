namespace JAAvila.FluentOperations.Config;

/// <summary>
/// Represents a temporary configuration scope. When disposed, restores the previous configuration.
/// Created via FluentOperationsConfig.Scope().
/// </summary>
public sealed class ConfigScope : IDisposable
{
    private readonly GlobalConfig _previous;
    private bool _disposed;

    internal ConfigScope(GlobalConfig previous)
    {
        _previous = previous;
    }

    /// <summary>
    /// Restores the previous configuration that was active before this scope was created.
    /// Subsequent calls after the first are no-ops.
    /// </summary>
    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        _disposed = true;
        GlobalConfig.RestoreConfig(_previous);
    }
}
