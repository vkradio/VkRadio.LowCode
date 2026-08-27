using VkRadio.LowCode.AppGen.Domain.PredefinedEntityInstances;
using VkRadio.LowCode.AppGen.Domain.PropertyDefinition.SystemFunctionalTypes;

namespace VkRadio.LowCode.AppGen.Domain.PropertyDefinition.ConcreteFunctionalTypes;

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

    public override IPropertyValue CreatePropertyValue() => new PropertyValue<SRefObject>
    {
        Definition = (PropertyDefinition)_propertyDefinition
    };
}
