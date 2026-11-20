using VkRadio.LowCode.AppGen.Domain.PropertyDefinition;

namespace VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.CSharp.Core.Type;

public class CSharpReferenceDomainTableType : CSharpReferenceDomainType
{
    public CSharpReferenceDomainTableType(bool supportNullableRefTypes, IPropertyDefinition propertyDefinition)
        : base(supportNullableRefTypes, "DataTable", false, propertyDefinition.GetEntityClassName())
    {
    }
}
