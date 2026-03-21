using System.Runtime.CompilerServices;
using JAAvila.FluentOperations.Handler;
using JAAvila.FluentOperations.Model;
using JAAvila.FluentOperations.Snapshot;

namespace JAAvila.FluentOperations.Extensions;

/// <summary>
/// Extension methods for <see cref="QualityReport"/> that enable snapshot-based regression testing.
/// </summary>
public static class QualityReportSnapshotExtensions
{
    /// <summary>
    /// Asserts that the <paramref name="report"/> matches the stored snapshot identified by
    /// <paramref name="snapshotName"/>. On the first run (when no snapshot file exists), the
    /// behavior depends on <see cref="SnapshotOptions.UpdateMode"/>:
    /// <list type="bullet">
    ///   <item><see cref="SnapshotUpdateMode.Manual"/> — throws, instructing you to call <see cref="UpdateSnapshot"/>.</item>
    ///   <item><see cref="SnapshotUpdateMode.CreateOnly"/> — creates the snapshot file and passes (baseline established).</item>
    ///   <item><see cref="SnapshotUpdateMode.AutoUpdate"/> — always overwrites the snapshot and passes.</item>
    /// </list>
    /// </summary>
    /// <param name="report">The quality report to compare.</param>
    /// <param name="snapshotName">A unique name for the snapshot (file stem, without extension).</param>
    /// <param name="options">Optional snapshot options. When <c>null</c>, defaults are used.</param>
    /// <param name="callerFilePath">Injected automatically by the compiler — do not supply manually.</param>
    public static void ShouldMatchSnapshot(
        this QualityReport report,
        string snapshotName,
        SnapshotOptions? options = null,
        [CallerFilePath] string callerFilePath = "")
    {
        ArgumentNullException.ThrowIfNull(report);
        ArgumentException.ThrowIfNullOrWhiteSpace(snapshotName);

        options ??= new SnapshotOptions();

        var (dir, filePath) = ResolvePaths(snapshotName, options, callerFilePath);

        if (!File.Exists(filePath))
        {
            switch (options.UpdateMode)
            {
                case SnapshotUpdateMode.Manual:
                    ExceptionHandler.Throw(
                        $"Snapshot file '{filePath}' does not exist. " +
                        $"Call report.UpdateSnapshot(\"{snapshotName}\") once to create the baseline snapshot file.");
                    return;

                case SnapshotUpdateMode.CreateOnly:
                case SnapshotUpdateMode.AutoUpdate:
                    WriteSnapshot(report, options, dir, filePath);
                    return;
            }
        }

        // File exists
        if (options.UpdateMode == SnapshotUpdateMode.AutoUpdate)
        {
            WriteSnapshot(report, options, dir, filePath);
            return;
        }

        var storedJson = File.ReadAllText(filePath);
        SnapshotSerializer.SnapshotData stored;

        try
        {
            stored = SnapshotSerializer.Deserialize(storedJson);
        }
        catch (InvalidOperationException ex)
        {
            ExceptionHandler.Throw(ex.Message);
            return;
        }

        var diff = SnapshotComparer.Compare(stored, report, snapshotName, options);

        if (!diff.IsMatch)
        {
            ExceptionHandler.Throw(diff.FormatReport());
        }
    }

    /// <summary>
    /// Writes the <paramref name="report"/> as a snapshot file, always overwriting any existing file.
    /// Use this method once to create or regenerate a baseline snapshot.
    /// </summary>
    /// <param name="report">The quality report to snapshot.</param>
    /// <param name="snapshotName">A unique name for the snapshot (file stem, without extension).</param>
    /// <param name="options">Optional snapshot options. When <c>null</c>, defaults are used.</param>
    /// <param name="callerFilePath">Injected automatically by the compiler — do not supply manually.</param>
    public static void UpdateSnapshot(
        this QualityReport report,
        string snapshotName,
        SnapshotOptions? options = null,
        [CallerFilePath] string callerFilePath = "")
    {
        ArgumentNullException.ThrowIfNull(report);
        ArgumentException.ThrowIfNullOrWhiteSpace(snapshotName);

        options ??= new SnapshotOptions();

        var (dir, filePath) = ResolvePaths(snapshotName, options, callerFilePath);
        WriteSnapshot(report, options, dir, filePath);
    }

    // -------------------------------------------------------------------------
    // Helpers
    // -------------------------------------------------------------------------

    private static (string dir, string filePath) ResolvePaths(
        string snapshotName, SnapshotOptions options, string callerFilePath)
    {
        // When SnapshotDirectory is an absolute path (e.g. a temp dir in tests), use it directly.
        // Otherwise, resolve relative to the caller's source file directory.
        string dir;
        if (Path.IsPathRooted(options.SnapshotDirectory))
        {
            dir = options.SnapshotDirectory;
        }
        else
        {
            var callerDir = Path.GetDirectoryName(callerFilePath) ?? Directory.GetCurrentDirectory();
            dir = Path.Combine(callerDir, options.SnapshotDirectory);
        }

        var filePath = Path.Combine(dir, snapshotName + ".json");
        return (dir, filePath);
    }

    private static void WriteSnapshot(
        QualityReport report, SnapshotOptions options, string dir, string filePath)
    {
        Directory.CreateDirectory(dir);
        var json = SnapshotSerializer.Serialize(report, options);
        File.WriteAllText(filePath, json);
    }
}
