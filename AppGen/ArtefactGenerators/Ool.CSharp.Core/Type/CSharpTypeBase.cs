using Ardalis.GuardClauses;
using VkRadio.LowCode.AppGen.Domain.PropertyDefinition;
using VkRadio.LowCode.AppGen.Domain.PropertyDefinition.ConcreteFunctionalTypes;

namespace VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.CSharp.Core.Type;

public abstract class CSharpTypeBase : IFormattable
{
    protected readonly string _typeName;

    protected CSharpTypeBase(string typeName) => _typeName = Guard.Against.NullOrEmpty(typeName, nameof(typeName));

    public string TypeName => _typeName;

    public abstract string GenerateLiteral(string? format);

    public override string ToString() => ToString(null, null);

    public string ToString(string? format, IFormatProvider? formatProvider = null) => GenerateLiteral(format);

    public abstract string? DefaultValue();
}

public static class CSharpTypeExtensions
{
    public static CSharpTypeBase GetCSharpType(this PropertyDefinition propertyDefinition, bool supportNullableRefTypes, bool nonValueFieldsAlwaysNullable = false)
    {
        Guard.Against.Null(propertyDefinition, nameof(propertyDefinition));

        CSharpTypeBase result;

        if (propertyDefinition.FunctionalType is PFTLink pftLink)
        {
            var (entityDefinition, isTable) = pftLink.GetEntityDefinition();

            if (!isTable)
            {
                result = new CSharpReferenceDomainType(supportNullableRefTypes, entityDefinition.GetEntityClassName(), propertyDefinition.FunctionalType.Nullable || nonValueFieldsAlwaysNullable);
            }
            else
            {
                result = new CSharpReferenceDomainTableType(supportNullableRefTypes, propertyDefinition);
            }
        }
        else
        {
            var type = propertyDefinition.GetPredefinedType();

            result = type!.Value.IsValueType()
                ? new CSharpValueType(type.Value, propertyDefinition.FunctionalType.Nullable)
                : new CSharpReferenceTechnicalType(supportNullableRefTypes, type.Value, propertyDefinition.FunctionalType.Nullable || nonValueFieldsAlwaysNullable);
        }

        return result;
    }
}
