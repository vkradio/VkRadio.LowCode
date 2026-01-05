using VkRadio.LowCode.AppGenerator.MetaModel.PredefinedDO;
using VkRadio.LowCode.AppGenerator.MetaModel.PropertyDefinition.SystemFunctionalTypes;

namespace VkRadio.LowCode.AppGenerator.MetaModel.PropertyDefinition.ConcreteFunctionalTypes;

/// <summary>
/// Abstract functional property type - relationship with other objects
/// </summary>
public abstract class PFTLink : PropertyFunctionalType
{
    public PFTLink()
    {
        _defaultValue = null;
        _nullable = true;
        _quantitative = false;
        _unique = false;
    }

    public override object ParseValueFromXmlString(string xmlString) => new SRefObject(new Guid(xmlString));

    public override IPropertyValue CreatePropertyValue()
    {
        return new PropertyValue<SRefObject>
        {
            Definition = (PropertyDefinition)_propertyDefinition
        };
    }
}
