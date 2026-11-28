using System.Xml.Linq;
using VkRadio.LowCode.AppGen.Domain.PredefinedEntityInstances;
using VkRadio.LowCode.AppGen.Domain.PropertyDefinition.SystemFunctionalTypes;

namespace VkRadio.LowCode.AppGen.Domain.PropertyDefinition.ConcreteFunctionalTypes;

/// <summary>
/// Abstract functional property type - relationship with other objects
/// </summary>
public abstract class PFTLink : PropertyFunctionalType
{
    public PFTLink(bool? nullable, bool? unique, XElement xElement)
        : base(nullable ?? true, unique ?? false, xElement)
    {
        DefaultValue = null;
        Quantitative = false;
    }

    public override object ParseValueFromXmlString(string xmlString) => new SRefObject(new Guid(xmlString));

    public override IPropertyValue CreatePropertyValue() => new PropertyValue<SRefObject>(PropertyDefinition);

    public abstract (EntityDefinition EntityDefinition, bool IsTable) GetEntityDefinition();
}
