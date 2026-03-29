using System.Text;
using JAAvila.FluentOperations.Common;
using JAAvila.FluentOperations.Handler;
using JAAvila.FluentOperations.Model;
using JAAvila.SafeTypes.Extension;
using JAAvila.SafeTypes.Manager;

namespace JAAvila.FluentOperations;

/// <summary>
/// Represents a scoped transactional context that accumulates validation failures instead of
/// throwing immediately. Multiple failures are collected and surfaced as a single composite
/// exception when the scope is disposed of. Supports nested scopes and async-safe isolation
/// via <see cref="AsyncLocal{T}"/>.
/// </summary>
public class TransactionalOperations : IDisposable
{
    private static readonly AsyncLocal<TransactionalOperations?> Instance = new();

    private const string Divider = "################";
    private readonly string _id = SafeCrypto.CreateIdentifier();
    private readonly TransactionalMode _mode;
    private TransactionalOperations? _parent;
    private readonly Func<string> _nameBuilder;
    private readonly List<string> _templates = [];
    private TransactionalStatus _status;
    private readonly string _headerIndicator = $"{Unicodes.ArrowCurvingRight} ";
    private string _header =
        "Unfinished operations have been found in the execution of the transaction:";

    /// <summary>
    /// Initializes a new transactional scope with the default mode
    /// (<see cref="TransactionalMode.AccumulateFailsAndDisposeThis"/>) and an auto-generated identifier.
    /// </summary>
    public TransactionalOperations()
        : this(() => null) { }

    /// <summary>
    /// Initializes a new transactional scope with the given name and the default mode.
    /// </summary>
    /// <param name="name">A human-readable label included in failure messages.</param>
    public TransactionalOperations(string name)
        : this(() => name) { }

    /// <summary>
    /// Initializes a new transactional scope with an auto-generated identifier and the specified mode.
    /// </summary>
    /// <param name="mode">Controls how the scope propagates failures to the parent and when it throws.</param>
    public TransactionalOperations(TransactionalMode mode)
        : this(() => null, mode) { }

    /// <summary>
    /// Initializes a new transactional scope with the given name and mode.
    /// </summary>
    /// <param name="name">A human-readable label included in failure messages.</param>
    /// <param name="mode">Controls how the scope propagates failures to the parent and when it throws.</param>
    public TransactionalOperations(string name, TransactionalMode mode)
        : this(() => name, mode) { }

    private TransactionalOperations(
        Func<string?> name,
        TransactionalMode mode = TransactionalMode.AccumulateFailsAndDisposeThis
    )
    {
        _parent = Instance.Value;
        _mode = mode;
        _status = TransactionalStatus.Success;
        Instance.Value = this;

        if (_parent is null)
        {
            _nameBuilder = () => name() ?? _id;
            return;
        }

        _nameBuilder = () =>
        {
            var pName = _parent._nameBuilder();
            var cName = name();

            if (pName.IsNullOrEmpty())
            {
                return cName ?? _id;
            }

            return $"{pName} {Unicodes.ArrowRight} {cName ?? _id}";
        };
    }

    /// <summary>
    /// Gets the innermost active <see cref="TransactionalOperations"/> scope on the current async context,
    /// or <c>null</c> if no scope is active.
    /// </summary>
    public static TransactionalOperations? Current => Instance.Value;

    /// <summary>
    /// Returns <c>true</c> when there is no active transactional scope, or when the current scope
    /// has not been interrupted by a fatal failure. Used by operation managers to short-circuit
    /// further assertions after a first-fail mode trigger.
    /// </summary>
    public static bool CanContinue()
    {
        if (Current is null)
        {
            return true;
        }

        return Current._status == TransactionalStatus.Success;
    }

    internal void SetHeader(Template template)
    {
        _header = template.ToString();
    }

    /// <summary>
    /// Gets the display name of this scope, composed of the scope's own name and any parent scope names.
    /// Falls back to an auto-generated identifier when no name was supplied.
    /// </summary>
    public string Name => _nameBuilder();

    /// <summary>
    /// Records a formatted failure message in the current scope and updates the scope status
    /// according to the active <see cref="TransactionalMode"/>.
    /// Throws if no transactional context is active or the template is null/empty.
    /// </summary>
    /// <param name="template">The fully formatted failure message to record.</param>
    public void HandleAddTemplate(string template)
    {
        SafeGuard.Throw.IfNull(Instance.Value, "Transactional", "No transactional context found.");
        SafeGuard.Throw.IfNullOrEmpty(template, "template", "Template cannot be null or empty.");

        Instance.Value!._templates.Add(template);

        switch (_mode)
        {
            case TransactionalMode.FirstFailAndDisposeThis:
                Instance.Value._status = TransactionalStatus.Interrupted;
                return;
            case TransactionalMode.FirstFailAndDisposeAll:
                Instance.Value._status = TransactionalStatus.Fail;
                return;
            case TransactionalMode.AccumulateFailsAndDisposeThis:
            case TransactionalMode.AccumulateFailsAndDisposeAll:
            default:
                break;
        }
    }

    /// <summary>
    /// Returns a read-only view of all failure messages accumulated in this scope so far.
    /// </summary>
    /// <returns>A read-only list of formatted failure message strings.</returns>
    public IReadOnlyList<string> GetTemplates() => _templates.AsReadOnly();

    /// <summary>
    /// Removes all accumulated failure messages from this scope without disposing of it.
    /// </summary>
    public void ClearTemplates() => _templates.Clear();

    private void AddTemplates(List<string> templates)
    {
        _templates.AddRange(templates);
    }

    private void ForceThrow()
    {
        ExceptionHandler.Throw(GenerateTemplate());
    }

    private string GenerateTemplate()
    {
        var sb = new StringBuilder();
        sb.Append($"{_headerIndicator}{_header}");
        sb.AppendLine();
        sb.AppendLine();
        //sb.Append(Unicodes.Tab);

        sb.AppendJoin(
            $"{Environment.NewLine}{Divider}{Environment.NewLine}",
            _templates.Select(x => x.AddIndentation())
        );

        return sb.ToString();
    }

    private void CheckIfShouldThrow()
    {
        if (_templates.Count == 0)
        {
            return;
        }

        ForceThrow();
    }

    /// <inheritdoc />
    public void Dispose()
    {
        Instance.Value = _parent;

        if (_parent is null)
        {
            CheckIfShouldThrow();
            return;
        }

        _parent.AddTemplates(_templates);

        if (
            _status == TransactionalStatus.Fail
            || _mode == TransactionalMode.AccumulateFailsAndDisposeAll
        )
        {
            _parent._status = TransactionalStatus.Fail;
        }

        /*if (_mode == TransactionalConfig.Mode.AccumulateFailsAndDisposeAll)
        {
            _parent.Dispose();
            return;
        }*/

        _parent = null;

        //_parent.AddTemplates(_templates);
    }
}
