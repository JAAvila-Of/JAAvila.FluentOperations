namespace JAAvila.FluentOperations.Model;

/// <summary>
/// Controls how snapshot files are created and updated during snapshot validation.
/// </summary>
public enum SnapshotUpdateMode
{
    /// <summary>
    /// Never auto-creates or auto-updates snapshot files.
    /// If the snapshot file is missing, the assertion throws instructing the user to call <c>UpdateSnapshot()</c>.
    /// This is the default the safest mode for CI environments.
    /// </summary>
    Manual = 0,

    /// <summary>
    /// Creates the snapshot file if it does not exist (first run establishes the baseline),
    /// but throws on any mismatch against an existing snapshot.
    /// </summary>
    CreateOnly = 1,

    /// <summary>
    /// Always overwrites the snapshot file with the current report.
    /// Use temporarily to regenerate snapshots after intentional changes.
    /// </summary>
    AutoUpdate = 2,
}
