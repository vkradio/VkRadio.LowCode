using System.Xml.Linq;
using VkRadio.LowCode.AppGen.Domain.Names;
using VkRadio.LowCode.AppGen.Domain.PredefinedEntityInstances;

namespace VkRadio.LowCode.AppGen.Domain.PropertyDefinition.ConcreteFunctionalTypes;

/// <summary>
/// Functional property type - integer number
/// </summary>
public class PFTInteger : PropertyFunctionalType
{
    public override string StringCode => C_STRING_CODE;

    public PFTInteger(bool? nullable, bool? unique, XElement xElement)
        : base(nullable ?? true, unique ?? false, xElement)
    {
        //DefaultValue = 0;
        DefaultValue = null;
        Quantitative = true;

        _defaultNames.Add(NaturalLanguageEnum.En, C_STRING_CODE);
        _defaultNames.Add(NaturalLanguageEnum.Ru, "целое число");
    }

    public const string C_STRING_CODE = "integer number";

    public override object ParseValueFromXmlString(string xmlString) => int.Parse(xmlString);

    public override IPropertyValue CreatePropertyValue() => new PropertyValue<int?>(PropertyDefinition);
}
