using System.Xml;
using System.Xml.Linq;
using VkRadio.LowCode.AppGen.Domain.PredefinedEntityInstances;
using VkRadio.LowCode.AppGen.Domain.PropertyDefinition.SystemFunctionalTypes;

namespace VkRadio.LowCode.AppGen.Domain.PropertyDefinition.ConcreteFunctionalTypes;

/// <summary>
/// Abstract functional type of a property containing date and/or time
/// </summary>
public abstract class PFTDateTime : PropertyFunctionalType
{
    /// <summary>
    /// Constructor
    /// </summary>
    protected PFTDateTime(bool? nullable, bool? unique, XElement xElement)
        : base(nullable ?? true, unique ?? false, xElement)
    {
        DefaultValue = null;
        Quantitative = false;
    }

    /// <summary>
    /// Extracting a value of date and time from an XML string
    /// </summary>
    /// <param name="xmlString">XML string</param>
    /// <returns>Typed property value, or a special literal &quot;runtime&quot;, if the current runtime value is required</returns>
    public override object? ParseValueFromXmlString(string xmlString)
    {
        return xmlString == C_RUNTIME_MARK
            ? new SDateTime()
            : new SDateTime(XmlConvert.ToDateTime(xmlString, XmlDateTimeSerializationMode.RoundtripKind));
    }

    /// <summary>
    /// Creating a typed prefab for storing a value
    /// </summary>
    /// <returns>Prefab of a property value</returns>
    public override IPropertyValue CreatePropertyValue() => new PropertyValue<SDateTime>(PropertyDefinition);

    /// <summary>
    /// String code defining a usage of a current runtime value
    /// </summary>
    public const string C_RUNTIME_MARK = "runtime";
}
