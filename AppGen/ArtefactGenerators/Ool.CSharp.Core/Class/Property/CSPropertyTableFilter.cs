using VkRadio.LowCode.AppGen.ArtefactGenerators.Core;
using VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.CSharp.Core.Type;
using VkRadio.LowCode.AppGen.Domain.Names;
using VkRadio.LowCode.AppGen.Domain.PropertyDefinition;
using VkRadio.LowCode.AppGen.Domain.PropertyDefinition.ConcreteFunctionalTypes;
using VkRadio.Patterns.Guards;

namespace VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.CSharp.Core.Class.Property;

public class CSPropertyTableFilter : CSPropertyPredefined, IPropertyOrField
{
    private readonly IPropertyDefinition _propertyDefinition;

    private CSharpReferenceDomainFilterType? _type;

    public static CSharpReferenceDomainFilterType GetPropertyType(IPropertyDefinition propDef) => new CSharpReferenceDomainFilterType(false, propDef);

    public static string PropertyName(PropertyDefinition propDef) => $"{NameHelper.NamesToPascalCase(propDef.Names, true)}Filter";

    private static string NameOfTableField(PropertyDefinition propDef)
    {
        IPropertyDefinition propInTable;

        if (propDef.FunctionalType is PFTBackReferencedTable refTable)
        {
            propInTable = refTable.RelationshipReference.OwnerPropertyDefinition;
        }
        else if (propDef.FunctionalType is PFTTablePart tblPart)
        {
            propInTable = tblPart.RelationshipTable.PropertyDefinitionInTable;
        }
        else
        {
            throw new GeneratorException($"Unsupported {nameof(PropertyFunctionalType)} ({propDef.FunctionalType.GetType().Name}) for collection property Id {propDef.Id}.");
        }

        var fieldName = $"{NameHelper.NameToUnderscoreSeparatedName(propInTable.Names)}_id";

        return fieldName;
    }

    public CSPropertyTableFilter(PropertyDefinition propDef, NaturalLanguageEnum preferNaturalLanguageForComments)
        : base(
            PropertyName(propDef),
            $"public virtual {GetPropertyType(propDef)} {PropertyName(propDef)} {{ get {{ return {GetPropertyType(propDef)}.CreateTableFilter(_id, \"{NameOfTableField(propDef)}\", VkRadio.LowCode.Libs.Orm.DbProviderFactory.Default); }} }}",
            new XmlComment((preferNaturalLanguageForComments == NaturalLanguageEnum.Ru ? "Фильтр для свойства " : "Filter for the property ") + NameHelper.GetLocalNameLowerCase(propDef.Names, preferNaturalLanguageForComments))
        )
    {
        _propertyDefinition = GuardRule.Forbid.Null(propDef, nameof(propDef));
    }

    public CSharpTypeBase Type
    {
        get
        {
            _type ??= GetPropertyType(_propertyDefinition);

            return _type;
        }
    }
}
