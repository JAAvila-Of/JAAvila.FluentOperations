using Grpc.Core;

namespace JAAvila.FluentOperations.Integration;

/// <summary>
/// Wraps an <see cref="IAsyncStreamReader{T}"/> and validates each message
/// according to the configured <see cref="StreamValidationMode"/>.
/// </summary>
/// <typeparam name="T">The request message type.</typeparam>
internal sealed class ValidatingStreamReader<T> : IAsyncStreamReader<T>
{
    private readonly IAsyncStreamReader<T> _inner;
    private readonly Func<T, Task> _validate;
    private readonly StreamValidationMode _mode;
    private bool _firstMessageValidated;

    internal ValidatingStreamReader(
        IAsyncStreamReader<T> inner,
        Func<T, Task> validate,
        StreamValidationMode mode)
    {
        _inner = inner;
        _validate = validate;
        _mode = mode;
    }

    /// <inheritdoc/>
    public T Current => _inner.Current;

    /// <inheritdoc/>
    public async Task<bool> MoveNext(CancellationToken cancellationToken)
    {
        var hasNext = await _inner.MoveNext(cancellationToken);

        if (!hasNext)
        {
            return false;
        }

        var shouldValidate = _mode switch
        {
            StreamValidationMode.EveryMessage => true,
            StreamValidationMode.FirstMessageOnly => !_firstMessageValidated,
            _ => false
        };

        if (shouldValidate)
        {
            _firstMessageValidated = true;
            await _validate(_inner.Current);
        }

        return true;
    }
}
