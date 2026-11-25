using VkRadio.LowCode.AppGen.Domain;
using VkRadio.LowCode.AppGen.Domain.Names;
using VkRadio.LowCode.AppGen.Domain.PropertyDefinition;
using VkRadio.LowCode.AppGen.Domain.PropertyDefinition.ConcreteFunctionalTypes;
using VkRadio.Patterns.Guards;

namespace VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.CSharp.Core.Type;

/// <summary>
/// Work with C# data types
/// </summary>
public static class TypeHelper
{
    public static CSharpTypeBase TypeOrDefault(this CSharpTypeBase? type) => type ?? new CSharpTypeHardCoded("void");

    /// <summary>
    /// Get XmlComment for a property definition of table type (collection)
    /// </summary>
    /// <param name="propDef"></param>
    /// <returns>XmlComment</returns>
    public static XmlComment GetXmlCommentForTablePropDef(PropertyDefinition propDef, NaturalLanguageEnum preferNaturalLanguageForComments) // TODO: Move it to a generalized generator of XmlComment or DocComment
    {
        XmlComment result;

        if (!(propDef.FunctionalType is PFTLink))
        {
            throw new NotImplementedException($"Cannot define XmlComment for non-reference PropertyDefinition Id {propDef.Id}, FunctionalType = {propDef.FunctionalType.GetType().Name}.");
        }
        else
        {
            if (propDef.FunctionalType is PFTBackReferencedTable ||
                propDef.FunctionalType is PFTTablePart)
            {
                var pftBackRefTable = propDef.FunctionalType as PFTBackReferencedTable;

                if (pftBackRefTable is not null)
                {
                    //result = new XmlComment(c_tableOfObjects + NameHelper.GetLocalNameUpperCase(((PropertyDefinition)pftBackRefTable.RelationshipReference.OwnerPropertyDefinition).OwnerDefinition.Names));
                    result = new XmlComment(NameHelper.GetLocalNameUpperCase(propDef.Names, preferNaturalLanguageForComments));
                }
                else
                {
                    //var pftTable = in_propDef.FunctionalType as PFTTablePart;
                    //result = new XmlComment(c_tableOfObjects + NameHelper.GetLocalNameUpperCase(pftTable.RelationshipTable.PropertyDefinitionInTable.OwnerDefinition.Names));
                    result = new XmlComment(NameHelper.GetLocalNameUpperCase(propDef.Names, preferNaturalLanguageForComments));
                }
            }
            else
            {
                throw new NotImplementedException($"Cannot define XmlComment for non-table PropertyDefinition Id {propDef.Id}, FunctionalType = {propDef.FunctionalType.GetType().Name}.");
            }
        }

        return result;
    }

    public static string GetEntityClassName(this EntityDefinition entityDef) => NameHelper.NamesToPascalCase(GuardRule.Forbid.Null(entityDef, nameof(entityDef)).Names);

    public static string? GetEntityClassName(this IPropertyDefinition propertyDefinition, bool returnNullIfNotApplicable = false)
    {
        GuardRule.Forbid.Null(propertyDefinition, nameof(propertyDefinition));

        var pftLink = propertyDefinition.FunctionalType as PFTLink;

        if (pftLink is null)
        {
            if (returnNullIfNotApplicable)
            {
                return null;
            }
            else
            {
                throw new ArgumentException($"{nameof(propertyDefinition)}.{nameof(propertyDefinition.FunctionalType)} should be {nameof(PFTLink)}", nameof(propertyDefinition));
            }
        }

        var (entityDefinition, _) = pftLink.GetEntityDefinition();

        return entityDefinition.GetEntityClassName();
    }

    public static TypeEnum? GetPredefinedType(this PropertyDefinition propDef, bool returnNullIfNotApplicable = false) => propDef.FunctionalType switch
    {
        PFTBoolean => TypeEnum.Bool,
        PFTDateTime => TypeEnum.DateTime,
        PFTDecimal => TypeEnum.Decimal,
        PFTInteger => TypeEnum.Int32,
        PFTString => TypeEnum.String,
        PFTUniqueCode => TypeEnum.Guid,
        _ => returnNullIfNotApplicable
            ? null
            : throw new ArgumentException($"{nameof(propDef)} has no mapping to one of predefined C# types", nameof(propDef))
    };
}
