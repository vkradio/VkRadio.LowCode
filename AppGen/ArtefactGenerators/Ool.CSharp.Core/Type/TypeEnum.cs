using VkRadio.LowCode.AppGen.ArtefactGenerators.Core;
using VkRadio.Patterns.Guards;

namespace VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.CSharp.Core.Type;

public enum TypeEnum
{
    [TypeEnumLiteral("int", true)]
    Int32,

    [TypeEnumLiteral("uint", true)]
    UInt32,

    [TypeEnumLiteral("float", true)]
    Float,

    [TypeEnumLiteral("double", true)]
    Double,

    [TypeEnumLiteral("byte", true)]
    Byte,

    [TypeEnumLiteral("short", true)]
    Short,

    [TypeEnumLiteral("ushort", true)]
    UShort,

    [TypeEnumLiteral("char", true)]
    Char,

    [TypeEnumLiteral("bool", true)]
    Bool,

    [TypeEnumLiteral("DateTime", true)]
    DateTime,

    [TypeEnumLiteral("Guid", true)]
    Guid,

    [TypeEnumLiteral("decimal", true)]
    Decimal,

    [TypeEnumLiteral("object", false)]
    Object,

    [TypeEnumLiteral("string", false)]
    String
}

public class TypeEnumLiteralAttribute : Attribute
{
    public string Literal { get; private set; }

    public bool IsValueType { get; private set; }

    public TypeEnumLiteralAttribute(string literal, bool isValueType)
    {
        Literal = GuardRule.Forbid.NullOrEmpty(literal, nameof(literal));
        IsValueType = isValueType;
    }
}

public static class TypeEnumOperations
{
    public static string ToLiteral(this TypeEnum typeEnum) => Enum
        .GetValues<TypeEnum>()
        .Where(x => x == typeEnum)
        .Select(x => x.GetAttribute<TypeEnumLiteralAttribute>()!)
        .Select(x => x.Literal)
        .First();

    public static bool IsValueType(this TypeEnum typeEnum) => Enum
        .GetValues<TypeEnum>()
        .Where(x => x == typeEnum)
        .Select(x => x.GetAttribute<TypeEnumLiteralAttribute>()!)
        .Select(x => x.IsValueType)
        .First();
}
