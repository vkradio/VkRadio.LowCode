namespace VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.CSharp.Core.Type;

public class CSharpReferenceType : CSharpTypeBase
{
    private readonly bool _supportNullableRefTypes;

    private readonly bool _isNullableByDomain;

    public CSharpReferenceType(bool supportNullableRefTypes, string typeName, bool isNullableByDomain)
        : base(typeName)
    {
        _supportNullableRefTypes = supportNullableRefTypes;

        _isNullableByDomain = isNullableByDomain;
    }

    public override string GenerateLiteral() => $"{_typeName}{(_isNullableByDomain && _supportNullableRefTypes ? "?" : string.Empty)}";
}
