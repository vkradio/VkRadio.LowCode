using VkRadio.LowCode.AppGen.Domain.Names;
using VkRadio.LowCode.AppGen.Domain.PredefinedEntityInstances;

namespace VkRadio.LowCode.AppGen.Domain.PropertyDefinition.ConcreteFunctionalTypes;

/// <summary>
/// Functional property type - integer number
/// </summary>
public class PFTInteger : PropertyFunctionalType
{
    public PFTInteger()
    {
        //_defaultValue = 0;
        _defaultValue = null;
        _nullable = true;
        _quantitative = true;
        _stringCode = C_STRING_CODE;
        _unique = false;

        _defaultNames.Add(NaturalLanguageEnum.En, C_STRING_CODE);
        _defaultNames.Add(NaturalLanguageEnum.Ru, "целое число");
    }

    public const string C_STRING_CODE = "integer number";

    public override object ParseValueFromXmlString(string xmlString) => int.Parse(xmlString);

    public override IPropertyValue CreatePropertyValue() => new PropertyValue<int?>
    {
        Definition = (PropertyDefinition)_propertyDefinition
    };
}
