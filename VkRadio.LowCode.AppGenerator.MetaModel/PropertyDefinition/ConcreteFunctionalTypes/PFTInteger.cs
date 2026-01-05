using VkRadio.LowCode.AppGenerator.MetaModel.Names;
using VkRadio.LowCode.AppGenerator.MetaModel.PredefinedDO;

namespace VkRadio.LowCode.AppGenerator.MetaModel.PropertyDefinition.ConcreteFunctionalTypes;

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

        _defaultNames.Add(HumanLanguageEnum.En, C_STRING_CODE);
        _defaultNames.Add(HumanLanguageEnum.Ru, "целое число");
    }

    public const string C_STRING_CODE = "integer number";

    public override object ParseValueFromXmlString(string xmlString) => int.Parse(xmlString);

    public override IPropertyValue CreatePropertyValue()
    {
        return new PropertyValue<int?>
        {
            Definition = (PropertyDefinition)_propertyDefinition
        };
    }
}
