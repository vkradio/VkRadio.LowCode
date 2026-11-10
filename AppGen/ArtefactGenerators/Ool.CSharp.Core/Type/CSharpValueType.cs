namespace VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.CSharp.Core.Type;

public class CSharpValueType : CSharpTypeBase
{
    private readonly bool _isNullable;

    public CSharpValueType(string typeName, bool isNullable)
        : base(typeName)
    {
        _isNullable = isNullable;
    }

    public override string GenerateLiteral() => $"{_typeName}{(_isNullable ? "?" : string.Empty)}";
}
