namespace JAAvila.FluentOperations.Connector;

/// <summary>
/// Connector returned by .Which() that provides access to a sub-value
/// extracted from the subject under test. Enables inspection of sub-properties
/// while maintaining the ability to return to the parent assertion chain via .And.
/// </summary>
/// <typeparam name="TManager">The parent manager type.</typeparam>
/// <typeparam name="TSubject">The type of the extracted sub-value.</typeparam>
public class AndWhichConnector<TManager, TSubject>
{
    private readonly TManager _parentManager;
    private readonly TSubject _subject;
    private readonly string _callerName;

    /// <summary>
    /// Represents an intermediary connector that facilitates method chaining
    /// between a parent manager and a resulting subject, often used in fluent-style APIs.
    /// </summary>
    /// <typeparam name="TManager">
    /// The type of the parent manager initiating the connector.
    /// </typeparam>
    /// <typeparam name="TSubject">
    /// The type of the subject managed or returned by the connector.
    /// </typeparam>
    public AndWhichConnector(TManager parentManager, TSubject subject, string callerName)
    {
        _parentManager = parentManager;
        _subject = subject;
        _callerName = callerName;
    }

    /// <summary>
    /// Returns to the parent manager to continue the original assertion chain.
    /// </summary>
    public TManager And => _parentManager;

    /// <summary>
    /// The extracted sub-value. Can be used with .Test() extension methods directly.
    /// Example: connector.Subject.Test().Be(5)
    /// </summary>
    public TSubject Subject => _subject;
}
