namespace JAAvila.FluentOperations.Model;

/// <summary>
/// Configuration options for snapshot-based validation via <c>ShouldMatchSnapshot</c>.
/// </summary>
public class SnapshotOptions
{
    /// <summary>
    /// The directory (relative to the calling test file) where snapshot JSON files are stored.
    /// Defaults to <c>__snapshots__</c>.
    /// </summary>
    public string SnapshotDirectory { get; init; } = "__snapshots__";

    /// <summary>
    /// When <c>true</c>, the <c>attemptedValue</c> field is included in the snapshot JSON and compared during matching.
    /// Defaults to <c>false</c>.
    /// </summary>
    public bool IncludeAttemptedValues { get; init; } = false;

    /// <summary>
    /// When <c>true</c>, failure messages are included in the snapshot JSON and compared during matching.
    /// Set to <c>false</c> to ignore message changes (useful when messages are volatile).
    /// Defaults to <c>true</c>.
    /// </summary>
    public bool IncludeMessages { get; init; } = true;

    /// <summary>
    /// Controls how snapshot files are created and updated.
    /// Defaults to <see cref="SnapshotUpdateMode.Manual"/>.
    /// </summary>
    public SnapshotUpdateMode UpdateMode { get; init; } = SnapshotUpdateMode.Manual;
}
