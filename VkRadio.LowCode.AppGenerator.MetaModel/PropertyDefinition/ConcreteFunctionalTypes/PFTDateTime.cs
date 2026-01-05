using System.Xml;
using VkRadio.LowCode.AppGenerator.MetaModel.PredefinedDO;
using VkRadio.LowCode.AppGenerator.MetaModel.PropertyDefinition.SystemFunctionalTypes;

namespace VkRadio.LowCode.AppGenerator.MetaModel.PropertyDefinition.ConcreteFunctionalTypes;

/// <summary>
/// Abstract functional type of a property containing date and/or time
/// </summary>
public abstract class PFTDateTime : PropertyFunctionalType
{
    /// <summary>
    /// Constructor
    /// </summary>
    public PFTDateTime()
    {
        _defaultValue = null;
        _nullable = true;
        _quantitative = false;
        _unique = false;
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
    public override IPropertyValue CreatePropertyValue()
    {
        return new PropertyValue<SDateTime>()
        {
            Definition = (PropertyDefinition)_propertyDefinition
        };
    }

    /// <summary>
    /// String code defining a usage of a current runtime value
    /// </summary>
    public const string C_RUNTIME_MARK = "runtime";
}
