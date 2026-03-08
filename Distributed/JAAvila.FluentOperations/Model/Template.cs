using System.Diagnostics.CodeAnalysis;

namespace JAAvila.FluentOperations.Model;

internal record Template(
    [StringSyntax("CompositeFormat")] string Because = "",
    params object[] Arguments)
{
    public static Template New(
        [StringSyntax("CompositeFormat")] string because = "",
        params object[] arguments
    ) => new(because, arguments);
    
    public override string ToString()
    {
        return HasArguments ? string.Format(Because, Arguments) : Because;
    }

    #region PRIVATE METHODS

    private bool HasArguments => Arguments.Length > 0;

    #endregion
}