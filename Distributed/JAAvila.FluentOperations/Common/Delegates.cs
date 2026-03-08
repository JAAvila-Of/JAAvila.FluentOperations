namespace JAAvila.FluentOperations.Common;

/// <summary>
/// Delegate used to evaluate an occurrence constraint. Returns <c>true</c> when the
/// <paramref name="actual"/> count satisfies the constraint defined by <paramref name="occurrence"/>.
/// </summary>
/// <param name="occurrence">The expected occurrence count as defined by the constraint.</param>
/// <param name="actual">The actual count observed in the collection.</param>
/// <returns><c>true</c> if the actual count satisfies the occurrence constraint; otherwise <c>false</c>.</returns>
public delegate bool OccurrenceDelegate(int occurrence, int actual);
