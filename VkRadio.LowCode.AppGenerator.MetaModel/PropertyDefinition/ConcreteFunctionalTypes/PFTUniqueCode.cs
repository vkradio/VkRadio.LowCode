using VkRadio.LowCode.AppGenerator.MetaModel.Names;
using VkRadio.LowCode.AppGenerator.MetaModel.PredefinedDO;
using VkRadio.LowCode.AppGenerator.MetaModel.PropertyDefinition.SystemFunctionalTypes;

namespace VkRadio.LowCode.AppGenerator.MetaModel.PropertyDefinition.ConcreteFunctionalTypes;

/// <summary>
/// Functional property type - unique code (GUID)
/// </summary>
public class PFTUniqueCode: PropertyFunctionalType
{
    const string C_GENERATE_MARK = "generate";

    public PFTUniqueCode()
    {
        _defaultValue = null;
        _nullable = true;
        _quantitative = false;
        _unique = true;
        _stringCode = C_STRING_CODE;

        _defaultNames.Add(HumanLanguageEnum.En, C_STRING_CODE);
        _defaultNames.Add(HumanLanguageEnum.Ru, "уникальный код");
    }

    public override object ParseValueFromXmlString(string xmlString)
    {
        return xmlString == C_GENERATE_MARK
            ? new SGuid()
            : new SGuid(Guid.Parse(xmlString));
    }

    public override IPropertyValue CreatePropertyValue()
    {
        return new PropertyValue<SGuid>
        {
            Definition = (PropertyDefinition)_propertyDefinition
        };
    }

    public const string C_STRING_CODE = "unique code";
}
