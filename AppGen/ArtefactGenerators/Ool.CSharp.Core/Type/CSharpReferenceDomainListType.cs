using VkRadio.LowCode.AppGen.Domain.PropertyDefinition;

namespace VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.CSharp.Core.Type;

public class CSharpReferenceDomainListType : CSharpReferenceDomainType
{
    public CSharpReferenceDomainListType(bool supportNullableRefTypes, string entityName)
        : base(supportNullableRefTypes, $"List<{entityName}>", false, entityName)
    {
    }

    public CSharpReferenceDomainListType(bool supportNullableRefTypes, PropertyDefinition propertyDefinition)
        : this(supportNullableRefTypes, propertyDefinition.GetEntityClassName()!)
    {
    }
}
