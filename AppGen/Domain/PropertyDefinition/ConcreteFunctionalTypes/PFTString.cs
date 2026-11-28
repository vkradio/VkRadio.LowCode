using System.Xml.Linq;
using VkRadio.LowCode.AppGen.Domain.PredefinedEntityInstances;

namespace VkRadio.LowCode.AppGen.Domain.PropertyDefinition.ConcreteFunctionalTypes;

/// <summary>
/// String functional property type (abstract class)
/// </summary>
public abstract class PFTString : PropertyFunctionalType
{
    private int? ReadIntValue(XElement xel, string propName) => int.TryParse(xel.Element(propName)?.Value, out var parsedValue) ? parsedValue : null;

    protected int? ReadMaxLength(XElement xel) => ReadIntValue(xel, "MaxLength");

    protected int? ReadMinLength(XElement xel) => ReadIntValue(xel, "MinLength");

    protected int? ReadDefaultMaxLength(XElement xel) => ReadIntValue(xel, "DefaultMaxLength");

    protected int? ReadDefaultMinLength(XElement xel) => ReadIntValue(xel, "DefaultMinLength");

    protected PFTString(bool? nullable, bool? unique, XElement xElement)
        : base(nullable ?? true, unique ?? false, xElement)
    {
        DefaultValue = null;
        Quantitative = false;

        MaxLength = ReadMaxLength(xElement) ?? default;
        MinLength = ReadMinLength(xElement) ?? default;
    }

    /// <summary>
    /// Default max lenght of a string
    /// </summary>
    public int DefaultMaxLength { get; protected set; }

    /// <summary>
    /// Default min length of a string
    /// </summary>
    public int DefaultMinLength { get; protected set; }

    /// <summary>
    /// Max length of a string
    /// </summary>
    public int MaxLength { get; protected set; }

    /// <summary>
    /// Min length of a string
    /// </summary>
    public int MinLength { get; protected set; }

    public override object ParseValueFromXmlString(string xmlString) => xmlString;

    public override IPropertyValue CreatePropertyValue() => new PropertyValue<string>(PropertyDefinition);
}
