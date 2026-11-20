using Ardalis.GuardClauses;

namespace VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.CSharp.Core.Type;

public class CSharpValueType : CSharpTypeBase
{
    private readonly bool _isNullable;

    public CSharpValueType(TypeEnum type, bool isNullable)
        : base(type.ToLiteral())
    {
        Guard.Against.Expression(x => !x.IsValueType(), type, $"{nameof(type)} should be value type", nameof(type));

        _isNullable = isNullable;
    }

    public override string GenerateLiteral(string? format) => $"{_typeName}{(_isNullable ? "?" : string.Empty)}";

    public override string? DefaultValue() => null;
}
