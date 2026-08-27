using System.Xml.Linq;
using VkRadio.LowCode.AppGen.Domain.PredefinedEntityInstances;

namespace VkRadio.LowCode.AppGen.Domain.PropertyDefinition.ConcreteFunctionalTypes;

/// <summary>
/// String functional property type (abstract class)
/// </summary>
public abstract class PFTString : PropertyFunctionalType
{
    protected int _defaultMaxLength;
    protected int _defaultMinLength;
    protected int _maxLength;
    protected int _minLength;

    public PFTString()
    {
        _defaultValue = null;
        _nullable = true;
        _quantitative = false;
        _unique = false;
    }

    protected override void InitExtendedParams(XElement xelPropertyDefinition)
    {
        var xel = xelPropertyDefinition.Element("MaxLength");

        if (xel is not null)
        {
            _maxLength = int.Parse(xel.Value);
        }

        xel = xelPropertyDefinition.Element("MinLength");

        if (xel is not null)
        {
            _minLength = int.Parse(xel.Value);
        }
    }

    /// <summary>
    /// Default max lenght of a string
    /// </summary>
    public int DefaultMaxLength => _defaultMaxLength;

    /// <summary>
    /// Default min length of a string
    /// </summary>
    public int DefaultMinLength => _defaultMinLength;

    /// <summary>
    /// Max length of a string
    /// </summary>
    public int MaxLength => _maxLength;

    /// <summary>
    /// Min length of a string
    /// </summary>
    public int MinLength => _minLength;

    public override object ParseValueFromXmlString(string xmlString) => xmlString;

    public override IPropertyValue CreatePropertyValue() => new PropertyValue<string>
    {
        Definition = (PropertyDefinition)_propertyDefinition
    };
}
