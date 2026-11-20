namespace VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.CSharp.Core.Type;

public class CSharpReferenceDomainType : CSharpReferenceType
{
    public CSharpReferenceDomainType(bool supportNullableRefTypes, string typeName, bool isNullableByDomain, string? entityName = null)
        : base(supportNullableRefTypes, typeName, isNullableByDomain)
    {
        EntityName = entityName ?? typeName;
    }

    public override string? DefaultValue() => _supportNullableRefTypes && !_isNullableByDomainOrSpecification
        ? "default!"
        : null;

    public string EntityName { get; private set; }
}
