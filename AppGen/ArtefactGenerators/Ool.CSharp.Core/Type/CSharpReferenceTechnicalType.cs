using VkRadio.Patterns.Guards;

namespace VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.CSharp.Core.Type;

public class CSharpReferenceTechnicalType : CSharpReferenceType
{
    public CSharpReferenceTechnicalType(bool supportNullableRefTypes, string typeName, bool isNullableBySpecification)
        : base(supportNullableRefTypes, typeName, isNullableBySpecification)
    {
    }

    public CSharpReferenceTechnicalType(bool supportNullableRefTypes, TypeEnum type, bool isNullableBySpecification)
        : base(supportNullableRefTypes, type.ToLiteral(), isNullableBySpecification)
    {
        GuardRule.Forbid.Expression(x => x.IsValueType(), type, $"{nameof(type)} should be reference type", nameof(type));
    }

    public override string? DefaultValue() => _supportNullableRefTypes && !_isNullableByDomainOrSpecification
        ? "default!"
        : null;
}
