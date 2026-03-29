using System.Xml;
using System.Xml.Linq;
using JAAvila.FluentOperations.Contract;

namespace JAAvila.FluentOperations.Validators;

/// <summary>
/// Validates that the string is valid XML.
/// </summary>
internal class StringBeXmlValidator(PrincipalChain<string?> chain) : IValidator, IRuleDescriptor
{
    public static StringBeXmlValidator New(PrincipalChain<string?> chain) => new(chain);

    public string Expected { get; } = null!;
    public string ResultValidation { get; set; } = null!;
    public string MessageKey => "String.BeXml";
    string IRuleDescriptor.OperationName => "BeXml";
    Type IRuleDescriptor.SubjectType => typeof(string);
    IReadOnlyDictionary<string, object> IRuleDescriptor.Parameters =>
        new Dictionary<string, object>();

    public bool Validate()
    {
        try
        {
            _ = XDocument.Parse(chain.GetValue()!);
            return true;
        }
        catch (XmlException)
        {
            ResultValidation = "The value was expected to be valid XML.";
            return false;
        }
    }

    public Task<bool> ValidateAsync() => Task.FromResult(Validate());
}
