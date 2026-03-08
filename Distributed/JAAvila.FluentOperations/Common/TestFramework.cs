using JAAvila.SafeTypes.Attributes;

namespace JAAvila.FluentOperations.Common;

/// <summary>
/// Represents the available test frameworks recognized and supported within the FluentOperations.
/// </summary>
internal enum TestFramework
{
    /// <summary>
    /// Represents the Machine.Specifications (MSpec) testing framework,
    /// which is a Behavior Driven Development (BDD) framework designed for .NET applications.
    /// </summary>
    [EnumKeyValue<string>("assembly", "machine.specifications")]
    [EnumKeyValue<string>("exception", "Machine.Specifications.SpecificationException")]
    [EnumBooleanValue(false)]
    MSpec,

    /// <summary>
    /// Represents the NUnit testing framework,
    /// a widely used unit testing framework for .NET applications
    /// supporting attributes-based testing and a rich set of assertions.
    /// </summary>
    [EnumKeyValue<string>("assembly", "nunit.framework")]
    [EnumKeyValue<string>("exception", "NUnit.Framework.AssertionException")]
    [EnumBooleanValue(false)]
    NUnit,

    /// <summary>
    /// Represents the MSTest testing framework, which is a unit testing framework provided by Microsoft
    /// and commonly used for testing .NET applications.
    /// </summary>
    [EnumKeyValue<string>("assembly", "microsoft.visualstudio.testplatform.testframework")]
    [EnumKeyValue<string>(
        "exception",
        "Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException"
    )]
    [EnumBooleanValue(false)]
    MsTest,

    /// <summary>
    /// Represents the TUnit testing framework, designed to provide testing capabilities,
    /// often emphasizing extensibility and integration for development practices.
    /// </summary>
    [EnumKeyValue<string>("assembly", "tunit.assertions")]
    [EnumKeyValue<string>("exception", "TUnit.Assertions.Exceptions.AssertionException")]
    [EnumBooleanValue(true)]
    TUnit,

    /// <summary>
    /// Represents the xUnit.net testing framework, version 2,
    /// a popular and extensible unit testing framework for .NET applications.
    /// </summary>
    [EnumKeyValue<string>("assembly", "xunit.assert")]
    [EnumKeyValue<string>("exception", "Xunit.Sdk.XunitException")]
    [EnumBooleanValue(true)]
    XUnit2,

    /// <summary>
    /// Represents the xUnit.net v3 testing framework, which is a modern, extensible,
    /// and open-source unit testing framework for .NET applications.
    /// </summary>
    [EnumKeyValue<string>("assembly", "xunit.v3.assert")]
    [EnumKeyValue<string>("exception", "Xunit.Sdk.XunitException")]
    [EnumBooleanValue(true)]
    XUnit3
}
