using System.Globalization;
using System.Xml.Linq;
using VkRadio.LowCode.AppGen.Domain.Names;
using VkRadio.LowCode.AppGen.Domain.PredefinedEntityInstances;

namespace VkRadio.LowCode.AppGen.Domain.PropertyDefinition.ConcreteFunctionalTypes;

/// <summary>
/// Functional property type - fixed point decimal number
/// </summary>
public class PFTDecimal : PropertyFunctionalType
{
    public override string StringCode => C_STRING_CODE;

    /// <summary>
    /// Fixed point decimal number constructor
    /// </summary>
    public PFTDecimal(bool? nullable, bool? unique, XElement xElement)
        : base(nullable ?? true, unique ?? false, xElement)
    {
        //DefaultValue = 0.0m;
        DefaultValue = null;
        Quantitative = true;

        _defaultNames.Add(NaturalLanguageEnum.En, C_STRING_CODE);
        _defaultNames.Add(NaturalLanguageEnum.Ru, "десятичное число");
    }

    public const string C_STRING_CODE = "decimal number";

    public override object ParseValueFromXmlString(string in_xmlString) => decimal.Parse(in_xmlString, CultureInfo.InvariantCulture);

    public override IPropertyValue CreatePropertyValue() => new PropertyValue<decimal?>(PropertyDefinition);
}
