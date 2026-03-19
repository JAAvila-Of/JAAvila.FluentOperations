using System.Text;
using JAAvila.FluentOperations.Common;
using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that all elements in the collection pass the assertions defined by the inspector callback.
/// Creates a child <see cref="TransactionalOperations"/> scope per element to capture assertion failures
/// from <c>.Test()</c> calls inside the inspector, then reports failures through the standard
/// validator pipeline.
/// </summary>
internal class CollectionInspectValidator<T>(
    PrincipalChain<IEnumerable<T>> chain,
    Action<T, int> inspector
) : IValidator
{
    public static CollectionInspectValidator<T> New(
        PrincipalChain<IEnumerable<T>> chain,
        Action<T, int> inspector
    ) => new(chain, inspector);

    public string Expected { get; } = string.Empty;
    public string ResultValidation { get; set; } = string.Empty;

    public bool Validate()
    {
        var collection = chain.GetValue();
        var allFailures = new List<(int Index, IReadOnlyList<string> Failures)>();

        var index = 0;
        foreach (var element in collection)
        {
            using var elementScope = new TransactionalOperations(
                $"Element[{index}]",
                TransactionalMode.AccumulateFailsAndDisposeThis
            );

            try
            {
                inspector(element, index);
            }
            catch (Exception ex)
            {
                // If the exception was an assertion failure, it was already captured
                // in the elementScope by ExceptionHandler.Handle(). If it was a
                // non-assertion exception (user code bug), record it explicitly.
                if (elementScope.GetTemplates().Count == 0)
                {
                    elementScope.HandleAddTemplate(
                        $"Inspector threw an unexpected exception: {ex.GetType().Name}: {ex.Message}"
                    );
                }
            }

            var templates = elementScope.GetTemplates();

            if (templates.Count > 0)
            {
                allFailures.Add((index, [.. templates]));
                // Clear templates BEFORE Dispose to prevent propagation to parent scope.
                // The validator controls reporting through ResultValidation + ExecutionEngine.
                elementScope.ClearTemplates();
            }

            index++;
        }

        if (allFailures.Count == 0)
        {
            return true;
        }

        var sb = new StringBuilder();
        sb.Append(
            $"The collection was expected to have all elements pass inspection, "
            + $"but {allFailures.Count} element(s) failed:"
        );

        foreach (var (failIndex, failures) in allFailures)
        {
            sb.AppendLine();
            sb.Append($"  Element[{failIndex}]:");
            foreach (var failure in failures)
            {
                sb.AppendLine();
                sb.Append($"    {failure}");
            }
        }

        ResultValidation = sb.ToString();
        return false;
    }

    public Task<bool> ValidateAsync()
    {
        return Task.FromResult(Validate());
    }
}
