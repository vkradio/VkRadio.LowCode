using System.Globalization;
using VkRadio.LowCode.AppGen.Domain.Names;
using VkRadio.LowCode.AppGen.Domain.PredefinedEntityInstances;

namespace VkRadio.LowCode.AppGen.Domain.PropertyDefinition.ConcreteFunctionalTypes;

/// <summary>
/// Functional property type - fixed point decimal number
/// </summary>
public class PFTDecimal : PropertyFunctionalType
{
    /// <summary>
    /// Fixed point decimal number constructor
    /// </summary>
    public PFTDecimal()
    {
        //_defaultValue = 0.0m;
        _defaultValue = null;
        _nullable = true;
        _quantitative = true;
        _stringCode = C_STRING_CODE;
        _unique = false;

        _defaultNames.Add(NaturalLanguageEnum.En, C_STRING_CODE);
        _defaultNames.Add(NaturalLanguageEnum.Ru, "десятичное число");
    }

    public const string C_STRING_CODE = "decimal number";

    public override object ParseValueFromXmlString(string in_xmlString) => decimal.Parse(in_xmlString, CultureInfo.InvariantCulture);

    public override IPropertyValue CreatePropertyValue() => new PropertyValue<decimal?>()
    {
        Definition = (PropertyDefinition)_propertyDefinition
    };
};
