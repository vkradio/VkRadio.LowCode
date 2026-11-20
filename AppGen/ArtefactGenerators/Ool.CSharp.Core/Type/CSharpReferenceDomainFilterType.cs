using VkRadio.LowCode.AppGen.Domain.PropertyDefinition;

namespace VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.CSharp.Core.Type;

public class CSharpReferenceDomainFilterType : CSharpReferenceDomainType
{
    public CSharpReferenceDomainFilterType(bool supportNullableRefTypes, string entityName)
        : base(supportNullableRefTypes, "FilterSimple", false, entityName)
    {
    }

    public CSharpReferenceDomainFilterType(bool supportNullableRefTypes, IPropertyDefinition propertyDefinition)
        : this(supportNullableRefTypes, propertyDefinition.GetEntityClassName()!)
    {
    }
}
